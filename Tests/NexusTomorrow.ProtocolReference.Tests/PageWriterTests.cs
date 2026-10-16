using NexusTomorrow.Network.Auth.Message.Model;
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
    public void AnAreaWithNothingVerifiedSaysItsNameIsOnlyAGrouping()
    {
        const string Grouping = "This grouping comes from where the declarations sit today. It claims nothing about what these messages do, and it changes when the client's areas are settled.";
        string[] blocks = PageWriter.AreaPage("Sample", [ReferenceMessage.Of(typeof(ServerFF22))], s_walk, s_docs, s_stamp).Split("\n\n");

        Assert.Equal("Every message in the Sample area: its opcode, direction, how far it is verified, and its fields.", blocks[1]);
        Assert.Equal(Grouping, blocks[2]);
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
}
