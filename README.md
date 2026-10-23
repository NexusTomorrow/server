# NexusTomorrow

A server for the WildStar client, build 16042, written in C# on .NET 10.

> **Note:** NexusTomorrow is in early development. The server takes your client from the login
> screen into the world: you can create a character, enter the world and move around, alone (see
> [Running the server](#running-the-server)). Other players, creatures and chat come next. The
> server core is being built in steps; see [Roadmap](#roadmap).

## Goals

- **The client is the reference.** Every behaviour has to match what the 16042 client sends,
  expects and checks. A behaviour is right when the client and play say so.
- **One process, simple to run.** One binary, one bring-up script, an admin console, and safe
  defaults: everything listens on loopback until you choose otherwise.
- **Wire bugs have nowhere to hide.** Each client message is declared once, with the width of every
  field, and its reader and writer are generated from that declaration.
- **Every change has a test,** and the tests run in public CI.

## What's here

| Project | What it holds |
|---|---|
| `NexusTomorrow.Cryptography` | SRP6 login, the STS's RC4, password and random helpers |
| `NexusTomorrow.GameTable` | Typed loading of the client's game tables (`.tbl`) and text tables |
| `NexusTomorrow.Game.Static` | The game's enums and flags, by system |
| `NexusTomorrow.IO` | Readers for the client's area files and the server's own map format (`.nfmap`) |
| `NexusTomorrow.Network.Codec` | The bit reader and writer, the declaration attributes, the layout types and the client facts table, on the hot-path rules |
| `NexusTomorrow.Network.Codec.Generator` | The source generator that writes each declared message's reader, writer and layout |
| `NexusTomorrow.Network` | The opcode list, the message attributes, and the messages every connection shares |
| `NexusTomorrow.Network.Sts` | Messages for the STS login protocol |
| `NexusTomorrow.Network.Auth` | Messages for the auth server |
| `NexusTomorrow.Network.World` | Messages for the world server |
| `NexusTomorrow.Shared` | Shared helpers: factories, configuration binding, the update timer |
| `NexusTomorrow.Transport` | Frames, envelopes and the packet cipher, on the hot-path rules |
| `NexusTomorrow.Gateway` | The client listeners: STS, login and world connections, the message catalog and the opcode table |
| `NexusTomorrow.Owners` | The account and realm owners: one thread each over a message queue |
| `NexusTomorrow.Maps` | The map side: map owners, the map scheduler and the timer wheel, on the hot-path rules |
| `NexusTomorrow.Persistence` | The PostgreSQL schemas, their migrations, and the write-behind queue |
| `NexusTomorrow.Commands` | Admin commands with their permissions and audit |
| `NexusTomorrow.Server` | The server: one process hosting all of the above |
| `NexusTomorrow.MapGenerator` | Tool: extracts tables from your client and generates map files |
| `NexusTomorrow.ClientConnector` | Tool: starts your client pointed at a server |
| `Tools/NexusTomorrow.ProtocolReference` | Writes the wiki's message reference, opcode table and command list from the declarations and the server's registries |

The solution is `Source/NexusTomorrow.slnx`, and the tests live under `Tests/`.

## Requirements

- The .NET 10 SDK from the 10.0.4xx band. `global.json` pins `10.0.400` and takes the latest
  patch in that band, so local builds and CI run the same compiler and analysers.
- Your own WildStar 16042 client. No client files, tables or maps are distributed here.
- Windows, to run `ClientConnector`. The solution builds on Windows and Linux; CI builds it on
  Ubuntu.

## Build and test

From the repository root:

```powershell
dotnet build Source/NexusTomorrow.slnx
dotnet test --solution Source/NexusTomorrow.slnx
```

Tests use xUnit v3 on Microsoft Testing Platform. CI runs the same suite on GitHub-hosted runners
through the `Tests` workflow, which runs only when a maintainer dispatches it. Pushes and pull
requests don't trigger it, so a pull request shows no checks until a maintainer runs them.

The database tests need a PostgreSQL 18 server: set `NEXUSTOMORROW_TEST_PG` to a connection string for a login that can create databases, and each test makes and drops its own. Without it those tests skip; set `NEXUSTOMORROW_TEST_PG_REQUIRED=1` to make a missing server a failure instead. CI starts the server for you.

## Running the server

The server takes your client from the login screen into the world, where your character stands
alone. Seeing other players comes next.

1. Install PostgreSQL 18 and create a database and a role that owns it.
2. Set two environment variables (keep both out of any file in this repository):

   ```powershell
   $env:Persistence__ConnectionString = "Host=localhost;Database=nexustomorrow;Username=nexustomorrow;Password=<password>"
   $env:Gateway__UnknownAccountSecret = [Convert]::ToBase64String([Security.Cryptography.RandomNumberGenerator]::GetBytes(32))
   ```

   Keep the secret the same across restarts.
3. Create the schemas: `dotnet run --project Source/NexusTomorrow.Server -- migrate`.
4. Point `Content:ClientDataPath` at the folder MapGenerator extracted your tables into (the one
   holding `tbl`), for example `$env:Content__ClientDataPath = "<output>"`. Without it, character
   creation offers nothing.
5. Give each faction a start location: the world it starts in, a zone and a point in that world,
   taken from your own client's data. The server enters a character only into an open-world map
   (the four classic starting zones are open world); a character whose world is held or an
   instance cannot enter yet, and the server says so in its log at start-up. For example, for one
   faction:

   ```powershell
   $env:Realm__StartLocations__0__FactionId = "<faction id>"
   $env:Realm__StartLocations__0__WorldId = "<world id>"
   $env:Realm__StartLocations__0__ZoneId = "<zone id>"
   $env:Realm__StartLocations__0__X = "<x>"
   $env:Realm__StartLocations__0__Y = "<y>"
   $env:Realm__StartLocations__0__Z = "<z>"
   ```
6. Start it: `dotnet run --project Source/NexusTomorrow.Server`. Every listener binds to
   `127.0.0.1` unless configured otherwise.
7. At the server's console, create an account: `account create <email> <password>`. A password
   with spaces goes in double quotes, as in `account create alice@example.com "correct horse battery"`;
   the quotes are not part of it, so a password cannot hold a double quote. `help` lists the
   commands.
8. Start your client with ClientConnector pointed at `127.0.0.1`, and log in with that account.

Where you are when you log out or exit the game from the Escape menu, or close the client, is
saved, and you enter there next time. Log Out returns you to character select; Exit Game quits.

`migrate` applies every schema's pending migrations and exits 0, or exits 1 with the error. The
server refuses to start while any schema has a pending migration, so run `migrate` again after
updating. To log every refused STS request and every game message a connection decodes, drops
or sends (type names only; transport pings are not logged), set
`${env:Logging__LogLevel__NexusTomorrow.Gateway} = "Debug"` before step 6; the braces keep
PowerShell from reading the dot as a property.

Settings come from `appsettings.json`, then environment variables (`__` stands for `:`), then the
command line. The ones you are most likely to change:

| Key | Default | Meaning |
|---|---|---|
| `Persistence:ConnectionString` | none | The PostgreSQL connection string. Required. |
| `Gateway:UnknownAccountSecret` | none | Base64 of at least 32 bytes. Required. |
| `Gateway:Sts`, `Gateway:Login`, `Gateway:World` | `127.0.0.1`, ports 6600, 23115, 24000 | Each listener's `BindAddress` and `Port`. The three ports must differ. |
| `Gateway:WorldPublicAddress` | `127.0.0.1` | The IPv4 address the login server gives the client for the world listener. |
| `Persistence:FirstIdBlockTimeout` | 30 s | How long start waits to take the first character id block. If the database cannot give one in time, start fails with an error instead of waiting. |
| `Gateway:ConnectionsPerAddress` | 8 | Connections one address may hold on each listener. Further ones are closed at once. |
| `Realm:Id`, `Realm:Name` | 1, `NexusTomorrow` | The realm's id (1 to 16383) and the name players see. |
| `Content:ClientDataPath` | none | The folder holding `tbl`. |
| `Cli:Enabled` | `true` | Whether the console reads commands. It stays off when standard input is redirected. |

A configuration that fails these checks stops the server at start, naming the key and never its
value. Stopping the server (Ctrl+C) stops accepting connections, then stops each map on its next
tick, which hands every character in it back with its location saved; then the realm and account
owners handle what they have queued, and the server waits for every queued save to commit, however
long the database takes. While it waits it logs the backlog every 5 seconds.

## Tools

### MapGenerator

Extracts the game tables (`.tbl`) and localisation files (`.bin`) from your client, and generates
the base map files (`.nfmap`) the server reads. Point it at your client's patch folder:

```powershell
dotnet run --project Source/NexusTomorrow.MapGenerator -- -i "<client>\Patch" -e -g -o "<output>"
```

| Option | Does |
|---|---|
| `-i`, `--patchPath` | The client's patch folder. Required. |
| `-e`, `--extract` | Extracts every game table and localisation file. |
| `-g`, `--generate` | Generates map files from the client's area files. |
| `-o`, `--output` | An existing folder for the output. Defaults to the current folder. |
| `--worldId`, `--gridX`, `--gridY` | Limits generation to one world, or one grid of it. |

Tables land in `tbl` and maps in `map` under the output folder. Both are built from your client, so
keep them out of this repository.

### ClientConnector

Starts your client with a server's address on its command line. It changes nothing in the client.

1. Build it and copy its output into your client's folder, beside `WildStar64.exe`.
2. Run `NexusTomorrow.ClientConnector.exe`, from that folder or anywhere else (a shortcut works too):
   it looks for the client and `config.json` beside itself, and stops with a message if no client is
   there. The first time, it asks for the server's host name and your language (`en` or `de`) and
   saves them to `config.json`.
3. Every later run reads `config.json` and starts the client straight away. To change servers, edit
   or delete `config.json`.

## Roadmap

The server core is built in steps. Each step gets its own plan once the step before it is done, so
every plan starts from code that exists.

| Step | Delivers | Status |
|---|---|---|
| 1. Repository | The carried-over libraries, renamed, with nullable reference types on, tests and CI | Done |
| 2. Codec | Every client message declared once with its field widths; readers and writers generated from the declarations and checked against the client's own layouts | Done; the audit of each message continues |
| 3. Core | The server process: account, realm and per-map owners, each single-threaded over its own state; timers; saves to PostgreSQL 18; the admin command layer; a test harness with headless clients | In progress: the client logs in, creates a character, enters the world, moves, and logs out or exits with its place saved; seeing other players is next |
| 4. Milestone one | Log in, create a character, enter the world, walk, see another player move, and `/say` | Planned |
| 5. Console | A web admin console and a player portal for accounts | Planned |
| 6. Systems | The content pipeline, then quests, achievements, economy and social; combat after | Planned |

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md). The rule that matters most: nothing taken from the client
goes into this repository, ever. To report a security problem, see [SECURITY.md](SECURITY.md).

Work lands on `dev`, the default branch, and `main` holds what has been seen working with the
client. [CHANGELOG.md](CHANGELOG.md) lists what `dev` holds that `main` doesn't yet.

## Credits

NexusTomorrow started from the `game_rework` branch of
[NexusForever](https://github.com/NexusForever/NexusForever) and keeps its full history, so every
original author's work stays credited. The two are separate projects: NexusTomorrow doesn't track
or merge from NexusForever, so report issues with this code here rather than there.

## Licence

GNU Affero General Public License v3.0; see [LICENSE](LICENSE). If you run a modified version as a
server other people connect to, the licence requires you to offer those people the source of the
version you run.

## Disclaimer

WildStar is a trademark of NCSOFT Corporation. NexusTomorrow is an independent project and is not
affiliated with or endorsed by NCSOFT or Carbine Studios.
