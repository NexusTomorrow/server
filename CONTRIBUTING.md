# Contributing to NexusTomorrow

Thanks for wanting to help. This page covers the one rule that protects the project, how to get a
change in, and the house style the code is held to.

## The rule: nothing from the client

Never commit anything taken from the WildStar client, and never paste it into an issue or a pull
request. That covers:

- table rows, database rows and maps;
- client strings;
- decompiled code, pseudocode and disassembly;
- captured packets and payloads.

Facts about the wire are fine when you write them as your own numbers: an opcode, a direction, a
field's width, a message's total size. Tools that read the client keep their output on your own
machine. Tests and logs follow the same rule, so no assertion or log line prints a table row, a
client offset or a decompiled constant.

A pull request that breaks this rule is closed, however good the rest of it is.

## Before you start

For anything bigger than a small fix, open an issue first and describe what you want to change.
The design is decided up front, and agreeing on it before you write code saves you from building
something that can't land.

Pull requests go against `main`. Keep each one to a single concern, and keep reformatting out of a
change that only touches nearby code.

## Building and testing

```powershell
dotnet build Source/NexusTomorrow.slnx
dotnet format Source/NexusTomorrow.slnx
dotnet test --solution Source/NexusTomorrow.slnx
```

`dotnet format` needs a build first: it loads the solution's source generators from the build's
output, and without them every generated member looks missing.

- Every behaviour change comes with a test. Tests use xUnit v3 on Microsoft Testing Platform and
  live under `Tests/`, in a project named after the one it tests
  (`NexusTomorrow.Cryptography.Tests`).
- Name a test for the behaviour it checks, such as `SessionKeyIsAlwaysSixtyFourBytes`.
- CI runs only when a maintainer dispatches the `Tests` workflow, so your pull request shows no
  checks until a maintainer runs them for it. Before it builds, it checks your commit messages and
  that no `.cs` or `.md` file holds an em dash; after the build, it checks the formatting.
- A workflow change uses only GitHub-owned actions, pinned by full commit SHA with the version in a
  comment, and keeps `permissions: contents: read`.

## Code style

The style follows Microsoft's
[C# coding conventions](https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions),
with a few rules of our own. `.editorconfig` carries every rule an analyser can check, so an editor
that reads it flags most problems as you type. The build treats every warning as an error, style
rules included, so a violation fails the build rather than waiting for review. Run
`dotnet format Source/NexusTomorrow.slnx` after a build and before you commit to fix the layout for
you; CI checks that it would change nothing.

If a rule genuinely doesn't fit some code, suppress it with its reason rather than working around
it: in `.editorconfig`, scoped to the files it covers, with a comment above saying why, or with a
`#pragma warning disable` whose reason sits on the line above it.

### Naming

- **Spelling:** American English in identifiers (`Initialize`, `Color`). A message or table field
  takes the client's own name where the client has one. Spelling in comments and prose is up to
  you.
- **Case:** PascalCase for types, members and constants, with an `I` prefix on interfaces;
  camelCase for parameters and locals.
- **Fields:** `_camelCase` for private and internal fields. Static ones are `s_camelCase` (and
  `t_camelCase` for `[ThreadStatic]`), so every touch of shared state stands out where it happens.
- **`this.`:** never. The `_` prefix already tells a field from a parameter.

### Layout

- **Namespaces** are file-scoped, with `using` directives above them and `System` first.
- **Braces** follow the Allman style. Leave them off only when every branch of an `if`/`else`
  chain is a single line.
- **No column alignment** and no `#region`. Trailing commas are welcome in enums and multi-line
  initialisers.
- **Fields** come first in a type. Visibility is always written out, as the first modifier.
- **Attributes** on types and methods go one per line.

### Language

- **Nullable reference types** are on in every project, and nullable warnings are errors. A lookup
  that can miss returns `T?`, and its callers handle the miss. Use `!` only where a null is
  impossible, with a comment naming why.
- **Required configuration** is validated where it is bound, because the configuration binder
  ignores `required`.
- **`var`** only when the type is plain on the same line, as with `new` or a cast.
- **Classes** are `sealed` unless they're built for inheritance, and fields are `readonly` whenever
  they can be.
- **Modern C#** where it reads better or is safer: collection expressions, `is null`, property
  patterns, `required` and `init`, `FrozenDictionary` and `ImmutableArray` for data built once. No
  primary constructors on services or managers.
- **Packages:** versions live only in `Directory.Packages.props`, pinned exactly.

### Hot paths

Code that runs on every tick or for every message lives in projects marked
`<HotPath>true</HotPath>`; today that's `NexusTomorrow.Network.Codec`. Hot code allocates nothing
in steady state, so in those projects the build refuses:

- LINQ, `Activator`, `Enum.ToObject` and `Convert`, which allocate or box;
- `Stream` and `MemoryStream`, since hot code reads and writes spans;
- `DateTime.Now` and `DateTime.UtcNow`, since time comes from the injected clock;
- `Thread.Sleep`, `Task.Result`, `Task.Wait` and `GC.Collect`, which block or stall the tick.

It also treats the performance analysers as errors there, so logging goes through
source-generated `LoggerMessage` methods. The full list, with a reason for each entry, is in
`Build/HotPath/`. What no analyser sees is still yours to avoid: lambdas that capture, boxing
through an interface or a generic, and exceptions used for control flow.

### Documentation

Every public type and member gets a `/// <summary>`. Data declarations are the exception: game
table row models and enums document the type, and a member only when its name can't carry its
meaning (units, a bit layout, why an unknown value is still unknown). Private members and test
methods need none.

### Comments

A comment states what is true and what proves it: the invariant, plus a `file:line`, a measured
count, or the guarantee that breaks without it. Leave out how the code got that way; that belongs
in the commit message. When you change code, update its comments in the same change.

Don't use em dashes in comments or Markdown: a comma, a colon, parentheses or two sentences do the
job. CI rejects a `.cs` or `.md` file that holds one.

### Line endings and encoding

Git stores text with LF and checks it out with CRLF on every system (`.gitattributes`). Every `.cs`
file is UTF-8 with a byte order mark; other text files have none.

## Commit messages

Write the headline in the imperative mood, with no full stop, in 72 characters or fewer. Leave a
blank line under it, then add any further points as `- ` lines, also in the imperative:

```text
Validate the client connector's configuration where it is bound

- Throw InvalidDataException naming the missing key when config.json lacks a host name or language.
- Stop writing a config.json from an empty answer.
```

The blank line matters: git treats everything above the first blank line as the subject, so
without it `git log --oneline` and GitHub run the whole message together as the title. CI checks
every commit since `main` for the blank line, the 72-character headline and the missing full stop.

`git blame` skips the repository-wide formatting commits listed in `.git-blame-ignore-revs`. GitHub
does this on its own; to do it locally, run:

```powershell
git config blame.ignoreRevsFile .git-blame-ignore-revs
```

## Licence

NexusTomorrow is licensed under the GNU Affero General Public License v3.0. By opening a pull
request, you agree that your contribution is licensed under the same terms.
