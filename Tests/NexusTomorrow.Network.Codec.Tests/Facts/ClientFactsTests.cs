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

    [Fact]
    public void TheTableHoldsTheRowsItsMetaClaimsInOpcodeOrder()
    {
        using var meta = JsonDocument.Parse(File.ReadAllText(WireFacts.MetaPath));
        Dictionary<int, WireFactsRow> rows = WireFacts.Load(WireFacts.TablePath);

        Assert.Equal(1, meta.RootElement.GetProperty("schema").GetInt32());
        Assert.Equal(16042, meta.RootElement.GetProperty("build").GetInt32());
        Assert.Equal(meta.RootElement.GetProperty("rows").GetInt32(), rows.Count);
        Assert.Equal([.. rows.Keys.Order()], [.. rows.Keys]);
        Assert.All(rows.Keys, opcode => Assert.InRange(opcode, 0, ushort.MaxValue));
    }

    [Fact]
    public void TheFixturesThatMirrorClientMessagesMatchTheClientFacts()
    {
        Dictionary<int, WireFactsRow> rows = WireFacts.Load(WireFacts.TablePath);
        WireLayout[] layouts = [LayoutOf<CastSpellFixture>(), LayoutOf<ArchiveUpdateFixture>(), LayoutOf<ChatFixture>()];

        List<string> problems = [.. layouts.SelectMany(layout => WireFactsComparison.Compare(layout, rows.GetValueOrDefault(layout.Opcode)))];

        Assert.True(problems.Count == 0, string.Join(Environment.NewLine, problems));
    }

    [Fact]
    public void TheBehaviourFixturesUseOpcodesTheClientDoesNotHave()
    {
        Dictionary<int, WireFactsRow> rows = WireFacts.Load(WireFacts.TablePath);

        Assert.All(WireMessages.Layouts.Where(layout => layout.Opcode >= 0xFF00), layout => Assert.False(rows.ContainsKey(layout.Opcode)));
    }
}
