using System.Text.Json;
using NexusTomorrow.Network.Codec.Tests.Fixtures;

namespace NexusTomorrow.Network.Codec.Tests.Facts;

public sealed class ClientFactsTests
{
    private static WireLayout LayoutOf<T>()
        where T : IWireMessage
    {
        return T.Layout;
    }

    private static int OpcodeOf(string line)
    {
        using var row = JsonDocument.Parse(line);
        return row.RootElement.GetProperty("op").GetInt32();
    }

    [Fact]
    public void TheTableHoldsTheRowsItsMetaClaimsInOpcodeOrder()
    {
        using var meta = JsonDocument.Parse(File.ReadAllText(WireFacts.MetaPath));
        Dictionary<int, WireFactsRow> rows = WireFacts.Load(WireFacts.TablePath);
        int[] fileOrder = [.. File.ReadLines(WireFacts.TablePath).Where(line => line.Length > 0).Select(OpcodeOf)];

        Assert.Equal(2, meta.RootElement.GetProperty("schema").GetInt32());
        Assert.Equal(16042, meta.RootElement.GetProperty("build").GetInt32());
        Assert.Equal(meta.RootElement.GetProperty("rows").GetInt32(), rows.Count);
        Assert.Equal(rows.Count, fileOrder.Length);
        Assert.Equal([.. fileOrder.Order()], fileOrder);
        Assert.All(rows.Keys, opcode => Assert.InRange(opcode, 0, ushort.MaxValue));
    }

    // A known side the client's layout could not be explored for keeps its widths and no tree; the meta counts those sides, so one losing
    // its tree shows up as a count that no longer matches.
    [Fact]
    public void EveryTreeGivesItsSideWidthsAndTheMetaCountsTheSidesWithout()
    {
        using var meta = JsonDocument.Parse(File.ReadAllText(WireFacts.MetaPath));
        var problems = new List<string>();
        int without = 0;
        foreach (WireFactsRow row in WireFacts.Load(WireFacts.TablePath).Values)
        {
            foreach ((string name, WireFactsSide? side, bool readerView) in new[] { ("c2s", row.ClientToServer, false), ("s2c", row.ServerToClient, true) })
            {
                if (side is null || side.Kind == "unknown")
                    continue;
                if (side.Tree is null)
                {
                    without++;
                    continue;
                }

                List<int>? widths = TreeWidths.EmptyPath(WireTree.Parse(side.Tree), readerView);
                if (side.Widths is not null && (widths is null || !widths.SequenceEqual(side.Widths)))
                    problems.Add($"0x{row.Opcode:X4} {name}: the tree gives [{(widths is null ? "none" : string.Join(", ", widths))}], the row [{string.Join(", ", side.Widths)}]");
            }
        }

        Assert.Empty(problems);
        Assert.Equal(meta.RootElement.GetProperty("sides_without_tree").GetInt32(), without);
    }

    [Fact]
    public void TheFixturesThatMirrorClientMessagesMatchTheClientFacts()
    {
        Dictionary<int, WireFactsRow> rows = WireFacts.Load(WireFacts.TablePath);
        WireLayout[] layouts = [LayoutOf<Client009AFixture>(), LayoutOf<Server0104Fixture>(), LayoutOf<Client01C3Fixture>()];

        List<string> problems = [.. layouts.SelectMany(layout => WireFactsComparison.Compare(layout, rows.GetValueOrDefault(layout.Opcode)))];

        Assert.True(problems.Count == 0, string.Join(Environment.NewLine, problems));
    }

    [Fact]
    public void TheBehaviorFixturesUseOpcodesTheClientDoesNotHave()
    {
        Dictionary<int, WireFactsRow> rows = WireFacts.Load(WireFacts.TablePath);

        Assert.All(WireMessages.Layouts.Where(layout => layout.Opcode >= 0xFF00), layout => Assert.False(rows.ContainsKey(layout.Opcode)));
    }
}
