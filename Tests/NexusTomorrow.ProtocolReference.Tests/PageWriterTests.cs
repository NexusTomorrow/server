using System.Reflection;
using NexusTomorrow.Commands;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Network.Auth.Message.Model;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.Message.Model;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.ProtocolReference.Tests.Message.Model.Sample;

namespace NexusTomorrow.ProtocolReference.Tests;

public sealed class PageWriterTests
{
    private static readonly PageStamp s_stamp = new("bootstrap", "abc1234", "2026-10-17");
    private static readonly LayoutWalk s_walk = new([typeof(SampleRequest).Assembly]);
    private static readonly DocText s_docs = DocText.Load([typeof(SampleRequest).Assembly]);

    [Fact]
    public void AnAreaPageMatchesTheHandWrittenPage()
    {
        const string expected = """
            ![reference, committed](https://img.shields.io/badge/reference-committed-2ea44f) ![verified 2026-10-17](https://img.shields.io/badge/verified-2026--10--17-6e7781) [![bootstrap abc1234](https://img.shields.io/badge/bootstrap-abc1234-6e7781)](https://github.com/NexusTomorrow/server/commit/abc1234)

            Every message in the Sample area: its opcode, direction, how far it is verified, and its fields.

            > [!NOTE]
            > Generated from the message declarations at bootstrap `abc1234` by `Tools/NexusTomorrow.ProtocolReference`. Don't edit it by hand: change the declaration's documentation and regenerate.

            ## Server to client

            ### `0xFF21` ServerFF21

            | Opcode | Direction | Verified to |
            |---|---|---|
            | `0xFF21` | server to client | unverified |

            | Field | Bits | Encoding |
            |---|---|---|
            | `Unknown0 count` | 4 | unsigned |
            | `Unknown0` | | array, counted by `Unknown0 count` |
            | `Unknown0[].Unknown0` | 32 | unsigned |
            | `Unknown0[].Unknown1` | varies | wide string |
            | `Unknown1` | 3 | signed; selects `Unknown2` |
            | `Unknown2` | | union, selected by `Unknown1` |
            | `Unknown2(1)` | | when `Unknown1` is 1; nothing more |
            | `Unknown2(-1)` | | when `Unknown1` is -1 |
            | `Unknown2(-1).Unknown0` | 16 | unsigned |

            ## Client to server

            ### `0xFF20` SampleRequest

            Asks for one page of the sample list.

            | Opcode | Direction | Verified to |
            |---|---|---|
            | `0xFF20` | client to server | fields |

            | Field | Bits | Encoding | Meaning |
            |---|---|---|---|
            | `Page` | 8 | unsigned | The page wanted, counted from 0. |
            | `Unknown1` | 4 | unsigned | The client gives this value no meaning. It always sends 0. |

            The server answers with `ServerFF21`.

            ## Where the code lives

            | Piece | Path |
            |---|---|
            | Message declarations | `Tests/NexusTomorrow.ProtocolReference.Tests/Message/Model/Sample/` |
            | This page's generator | `Tools/NexusTomorrow.ProtocolReference/` |
            """;

        string page = PageWriter.AreaPage("Sample", [ReferenceMessage.Of(typeof(SampleRequest)), ReferenceMessage.Of(typeof(ServerFF21))], s_walk, s_docs, s_stamp);

        Assert.Equal(expected.ReplaceLineEndings("\n") + "\n", page);
    }

    [Fact]
    public void AnUnverifiedMessageRendersItsLayoutOnly()
    {
        string page = PageWriter.AreaPage("Sample", [ReferenceMessage.Of(typeof(ServerFF22))], s_walk, s_docs, s_stamp);

        Assert.DoesNotContain("A copy of", page, StringComparison.Ordinal);
        Assert.DoesNotContain("Nothing reads these remarks", page, StringComparison.Ordinal);
        Assert.DoesNotContain("A summary the page leaves out", page, StringComparison.Ordinal);
        Assert.DoesNotContain("| Meaning |", page, StringComparison.Ordinal);
        Assert.Contains("| `0xFF22` | server to client | unverified |", page, StringComparison.Ordinal);
    }

    [Fact]
    public void AnAreaWithNothingVerifiedClaimsNothingAboutItsMessages()
    {
        const string Grouping = "This area groups messages by the client feature they belong to; a message may sit here provisionally until its reading confirms the area. None is verified to its purpose yet, so this page claims nothing about what they do.";
        string[] blocks = PageWriter.AreaPage("Sample", [ReferenceMessage.Of(typeof(ServerFF22))], s_walk, s_docs, s_stamp).Split("\n\n");

        Assert.Equal("Every message in the Sample area: its opcode, direction, how far it is verified, and its fields.", blocks[1]);
        Assert.Equal(Grouping, blocks[2]);
        Assert.StartsWith("> [!NOTE]", blocks[3], StringComparison.Ordinal);
    }

    [Fact]
    public void TheUnsettledPageSaysItsMessagesHaveNoAreaYet()
    {
        const string Waiting = "These messages wait here until the client feature they belong to is found. This page claims nothing about what they do.";
        string[] blocks = PageWriter.AreaPage("Unsettled", [ReferenceMessage.Of(typeof(ServerFF22))], s_walk, s_docs, s_stamp).Split("\n\n");

        Assert.Equal(Waiting, blocks[2]);
        Assert.StartsWith("> [!NOTE]", blocks[3], StringComparison.Ordinal);
    }

    [Fact]
    public void TheIndexListsEveryMessageInOpcodeOrder()
    {
        string page = PageWriter.IndexPage([ReferenceMessage.Of(typeof(ServerFF21)), ReferenceMessage.Of(typeof(SampleRequest))], s_stamp);
        string[] lines = page.Split("\n");

        Assert.StartsWith("![reference, committed]", lines[0], StringComparison.Ordinal);
        int first = Array.IndexOf(lines, "| `0xFF20` | [SampleRequest](Messages-Sample#0xff20-samplerequest) | client to server | fields | [Sample](Messages-Sample) |");
        int second = Array.IndexOf(lines, "| `0xFF21` | [ServerFF21](Messages-Sample#0xff21-serverff21) | server to client | unverified | [Sample](Messages-Sample) |");
        Assert.True(first > 0, "the SampleRequest row is missing");
        Assert.Equal(first + 1, second);
        Assert.Contains("| [Sample](Messages-Sample) | 2 | 1 |", lines);
        Assert.EndsWith("\n", page, StringComparison.Ordinal);
        Assert.DoesNotContain("\r", page, StringComparison.Ordinal);
    }

    [Fact]
    public void AreasComeFromTheModelFolder()
    {
        Assert.Equal("Pregame", Areas.AreaOf(typeof(LoginQueueStatus)));
        Assert.Equal("Pregame", Areas.AreaOf(typeof(LoginRequest)));
        Assert.Equal("Pregame", Areas.AreaOf(typeof(ServerHello)));
        Assert.Equal("Sample", Areas.AreaOf(typeof(SampleRequest)));
    }

    [Fact]
    public void TheOpcodeTablePageListsEachRowWithItsLinkedMessageAndLimits()
    {
        OpcodeRow[] rows = [new(0xFF20, MessageOwner.Realm, MessagePriority.Normal, 2, OverLimitAction.Drop)];
        string[] blocks = PageWriter.OpcodeTablePage(rows, [ReferenceMessage.Of(typeof(SampleRequest))], s_stamp).Split("\n\n");

        Assert.Equal("Every client message the gateway admits, with the owner it goes to and its limits.", blocks[1]);
        Assert.Equal(
            "> [!NOTE]\n> Generated from the opcode table at bootstrap `abc1234` by `Tools/NexusTomorrow.ProtocolReference`. Don't edit it by hand: change the table and regenerate.",
            blocks[2]);
        Assert.Equal(
            "| Opcode | Message | Owner | Tag | Priority | Rate per second | Past the limit | Admitted at |\n|---|---|---|---|---|---|---|---|\n"
            + "| `0xFF20` | [SampleRequest](Messages-Sample#0xff20-samplerequest) | Realm | untagged | normal | 2 | drop | fields |",
            blocks[3]);
        Assert.Contains("| The opcode table | `Source/NexusTomorrow.Gateway/Catalog/OpcodeTable.cs` |", blocks[5], StringComparison.Ordinal);
    }

    [Fact]
    public void TheOpcodeTablePageNamesAHighPriorityAndADisconnect()
    {
        OpcodeRow[] rows = [new(0xFF20, MessageOwner.Account, MessagePriority.High, 1, OverLimitAction.Disconnect)];
        string page = PageWriter.OpcodeTablePage(rows, [ReferenceMessage.Of(typeof(SampleRequest))], s_stamp);

        Assert.Contains("| `0xFF20` | [SampleRequest](Messages-Sample#0xff20-samplerequest) | Account | untagged | high | 1 | disconnect | fields |", page, StringComparison.Ordinal);
    }

    [Fact]
    public void TheOpcodeTablePageShowsATaggedRowsTagAndMarksAnUntaggedRow()
    {
        OpcodeRow[] rows =
        [
            new(0xFF20, MessageOwner.Map, MessagePriority.Normal, 2, OverLimitAction.Drop, Tag: 0x03),
            new(0x07E0, MessageOwner.Realm, MessagePriority.Normal, 2, OverLimitAction.Drop),
        ];
        ReferenceMessage[] messages = [ReferenceMessage.Of(typeof(SampleRequest)), ReferenceMessage.Of(typeof(CharacterListRequest))];
        string[] blocks = PageWriter.OpcodeTablePage(rows, messages, s_stamp).Split("\n\n");

        Assert.Equal(
            "| Opcode | Message | Owner | Tag | Priority | Rate per second | Past the limit | Admitted at |\n|---|---|---|---|---|---|---|---|\n"
            + "| `0x07E0` | [CharacterListRequest](Messages-Pregame#0x07e0-characterlistrequest) | Realm | untagged | normal | 2 | drop | fields |\n"
            + "| `0xFF20` | [SampleRequest](Messages-Sample#0xff20-samplerequest) | Map | `0x03` | normal | 2 | drop | fields |",
            blocks[3]);
    }

    [Fact]
    public void TheOpcodeTablePageExplainsPurposeOnlyWhenARowIsAdmittedAtIt()
    {
        ReferenceMessage[] messages = [ReferenceMessage.Of(typeof(SampleRequest)), ReferenceMessage.Of(typeof(CharacterListRequest))];
        var fieldsRow = new OpcodeRow(0x07E0, MessageOwner.Realm, MessagePriority.Normal, 2, OverLimitAction.Drop);
        var purposeRow = new OpcodeRow(0xFF20, MessageOwner.Map, MessagePriority.High, 60, OverLimitAction.Drop, Tag: 0x03, ReadsVerifiedMembers: true);

        string[] both = PageWriter.OpcodeTablePage([fieldsRow, purposeRow], messages, s_stamp).Split("\n\n");
        string[] fieldsAlone = PageWriter.OpcodeTablePage([fieldsRow], messages, s_stamp).Split("\n\n");

        Assert.Equal(
            "| Opcode | Message | Owner | Tag | Priority | Rate per second | Past the limit | Admitted at |\n|---|---|---|---|---|---|---|---|\n"
            + "| `0x07E0` | [CharacterListRequest](Messages-Pregame#0x07e0-characterlistrequest) | Realm | untagged | normal | 2 | drop | fields |\n"
            + "| `0xFF20` | [SampleRequest](Messages-Sample#0xff20-samplerequest) | Map | `0x03` | high | 60 | drop | purpose |",
            both[3]);
        Assert.Equal("A row admitted at `purpose` is handled before its whole message reaches `fields`: every member its handler reads is verified.", both[4]);
        Assert.Equal("## Where the code lives", both[5]);

        Assert.Equal(
            "| Opcode | Message | Owner | Tag | Priority | Rate per second | Past the limit | Admitted at |\n|---|---|---|---|---|---|---|---|\n"
            + "| `0x07E0` | [CharacterListRequest](Messages-Pregame#0x07e0-characterlistrequest) | Realm | untagged | normal | 2 | drop | fields |",
            fieldsAlone[3]);
        Assert.Equal("## Where the code lives", fieldsAlone[4]);
        Assert.DoesNotContain(fieldsAlone, block => block.Contains("`purpose`", StringComparison.Ordinal));
    }

    [Fact]
    public void AnOpcodeRowWithNoClientMessageStopsThePage()
    {
        OpcodeRow[] rows = [new(0xFF21, MessageOwner.Realm, MessagePriority.Normal, 1, OverLimitAction.Disconnect)];
        Assert.Throws<InvalidOperationException>(() => PageWriter.OpcodeTablePage(rows, [ReferenceMessage.Of(typeof(ServerFF21))], s_stamp));
    }

    [Fact]
    public void TheCommandsPageListsEveryCommandAndHelpInNameOrderWithSecretsMarked()
    {
        CommandDefinition[] commands =
        [
            new()
            {
                Name = "zone reset",
                Description = "Reset a zone.",
                Permission = "zone.reset",
                ConsoleOnly = true,
                Parameters = [new("zone", "the zone"), new("why", "the reason", Optional: true, Rest: true)],
                ExecuteAsync = Never,
            },
            new()
            {
                Name = "account create",
                Description = "Create an account.",
                Permission = "account.create",
                Parameters = [new("email", "the login name"), new("password", "the password", Secret: true)],
                ExecuteAsync = Never,
            },
        ];
        string[] blocks = PageWriter.CommandsPage(commands, s_stamp).Split("\n\n");

        Assert.Equal("Every admin command, the permission it needs, and whether it's console-only.", blocks[1]);
        Assert.Equal(
            "| Command | Permission | Console only | Does |\n|---|---|---|---|\n"
            + "| `account create <email> <password, secret>` | `account.create` | No | Create an account. |\n"
            + "| `help` | none | No | List every command you may run, with its usage. |\n"
            + "| `zone reset <zone> [why...]` | `zone.reset` | Yes | Reset a zone. |",
            blocks[3]);
    }

    [Fact]
    public void ARegisteredHelpCommandReplacesTheBuiltInRow()
    {
        CommandDefinition[] commands =
        [
            new() { Name = "help", Description = "Show the house help.", Permission = null, Parameters = [], ExecuteAsync = Never },
        ];
        string table = PageWriter.CommandsPage(commands, s_stamp).Split("\n\n")[3];

        Assert.Equal("| Command | Permission | Console only | Does |\n|---|---|---|---|\n| `help` | none | No | Show the house help. |", table);
    }

    [Fact]
    public void ATypeOutsideEveryModelNamespaceHasNoArea()
    {
        Assert.Throws<InvalidOperationException>(() => Areas.AreaOf(typeof(PageWriterTests)));
    }

    [Fact]
    public void NoWorldMessageSitsAtTheModelRoot()
    {
        string[] outsideAnArea = [.. typeof(global::NexusTomorrow.Network.World.WireMessages).Assembly.GetTypes()
            .Where(type => type.GetCustomAttribute<WireMessageAttribute>() is not null)
            .Where(type => !(type.Namespace ?? "").Contains(".Message.Model.", StringComparison.Ordinal))
            .Select(type => type.FullName ?? type.Name)
            .Order(StringComparer.Ordinal)];
        Assert.Empty(outsideAnArea);
    }

    private static Task<CommandReply> Never(CommandInvocation invocation, CancellationToken cancellationToken) =>
        throw new InvalidOperationException("A page never runs a command.");
}
