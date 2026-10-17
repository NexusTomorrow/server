# NexusTomorrow

A server for the WildStar client, build 16042, written in C# on .NET 10.

> **Note:** NexusTomorrow is in early development and does not run a game server yet. This
> repository holds the libraries the server is built on, two tools that work with your client, and
> the build and test baseline. The server core is being built in steps; see [Roadmap](#roadmap).

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
| `NexusTomorrow.Network` | Bit-level packet reading and writing, and the message framing shared by every server |
| `NexusTomorrow.Network.Sts` | Messages for the STS login protocol |
| `NexusTomorrow.Network.Auth` | Messages for the auth server |
| `NexusTomorrow.Network.World` | Messages for the world server |
| `NexusTomorrow.Shared` | Shared helpers: factories, configuration binding, the update timer |
| `NexusTomorrow.Transport` | Frames, envelopes and the packet cipher, on the hot-path rules |
| `NexusTomorrow.Gateway` | The connection edge: the message catalog, the opcode table and the inbound decoder that admit a client message |
| `NexusTomorrow.Owners` | The owner base: one thread over one queue per owner, and the watchdog that reports one that stops draining |
| `NexusTomorrow.Persistence` | One EF Core context per PostgreSQL schema (`account`, `realm`, `ops`) and their migrations |
| `NexusTomorrow.MapGenerator` | Tool: extracts tables from your client and generates map files |
| `NexusTomorrow.ClientConnector` | Tool: starts your client pointed at a server |

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
2. Run `NexusTomorrow.ClientConnector.exe`. The first time, it asks for the server's host name and
   your language (`en` or `de`) and saves them to `config.json`.
3. Every later run reads `config.json` and starts the client straight away. To change servers, edit
   or delete `config.json`.

## Roadmap

The server core is built in steps. Each step gets its own plan once the step before it is done, so
every plan starts from code that exists.

| Step | Delivers | Status |
|---|---|---|
| 1. Repository | The carried-over libraries, renamed, with nullable reference types on, tests and CI | Done |
| 2. Codec | Every client message declared once with its field widths; readers and writers generated from the declarations and checked against the client's own layouts | In progress |
| 3. Core | The server process: account, realm and per-map owners, each single-threaded over its own state; timers; saves to PostgreSQL 18; the admin command layer; a test harness with headless clients | Planned |
| 4. Milestone one | Log in, create a character, enter the world, walk, see another player move, and `/say` | Planned |
| 5. Console | A web admin console and a player portal for accounts | Planned |
| 6. Systems | The content pipeline, then quests, achievements, economy and social; combat after | Planned |

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md). The rule that matters most: nothing taken from the client
goes into this repository, ever. To report a security problem, see [SECURITY.md](SECURITY.md).

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
