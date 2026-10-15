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
dotnet test --solution Source/NexusTomorrow.slnx
```

- Every behaviour change comes with a test. Tests use xUnit v3 on Microsoft Testing Platform and
  live under `Tests/`, in a project named after the one it tests
  (`NexusTomorrow.Cryptography.Tests`).
- Name a test for the behaviour it checks, such as `SessionKeyIsAlwaysSixtyFourBytes`.
- CI runs only when a maintainer dispatches the `Tests` workflow, so your pull request shows no
  checks until a maintainer runs them for it.
- A workflow change uses only GitHub-owned actions, pinned by full commit SHA with the version in a
  comment, and keeps `permissions: contents: read`.

## Code style

`.editorconfig` carries the formatting, and an editor that reads it handles most of this for you.
The points it can't enforce:

- **Spelling:** British English in identifiers, comments and commit messages (`initialise`,
  `customisation`, `neighbourhood`).
- **Nullable reference types** are on in every project, and nullable warnings are errors. A lookup
  that can miss returns `T?`, and its callers handle the miss. Use `!` only where a null is
  impossible, with a comment naming why.
- **Required configuration** is validated where it is bound, because the configuration binder
  ignores `required`.
- **Braces:** Allman style, with no braces round a single-line body.
- **Namespaces** are block-scoped, never file-scoped.
- **Documentation:** a `/// <summary>` on every public member, interface members included. Private
  members and test methods need none.
- **Naming:** PascalCase for types and public members; lowerCamel for locals, parameters and
  non-public fields, with no `_` prefix. `this.` only to tell a field from a parameter of the same
  name.
- **`var`** only when the type is plain on the same line, as with `new` or a cast.
- **Enums:** values aligned on the `=`, in value order, with no comma after the last.
- **Attributes** sit on their own line, except on parameters.
- **Member order:** constants and static fields, instance fields (readonly first), properties,
  constructors, interface implementations, other members, nested types. Injected fields and the
  constructor that sets them sit in `#region Dependency Injection`.
- **Modern C#** where it reads better or is safer: collection expressions, `is null`, property
  patterns, `required` and `init`, `FrozenDictionary` and `ImmutableArray` for data built once. No
  primary constructors on services or managers.
- **Packages:** versions live only in `Directory.Packages.props`, pinned exactly.

### Comments

A comment states what is true and what proves it: the invariant, plus a `file:line`, a measured
count, or the guarantee that breaks without it. Leave out how the code got that way; that belongs
in the commit message. When you change code, update its comments in the same change.

### Line endings and encoding

Git stores text with LF and checks it out with CRLF on every system (`.gitattributes`). Every `.cs`
file is UTF-8 with a byte order mark; other text files have none.

## Commit messages

The headline is one past-tense sentence ending in a full stop. Any further points go straight
under it as past-tense `- ` lines, with no blank line between:

```text
Validated the client connector's configuration where it is bound.
- Threw InvalidDataException naming the missing key when config.json lacks a host name or language.
- Stopped writing a config.json from an empty answer.
```

`git blame` skips the repository-wide formatting commits listed in `.git-blame-ignore-revs`. GitHub
does this on its own; to do it locally, run:

```powershell
git config blame.ignoreRevsFile .git-blame-ignore-revs
```

## Licence

NexusTomorrow is licensed under the GNU Affero General Public License v3.0. By opening a pull
request, you agree that your contribution is licensed under the same terms.
