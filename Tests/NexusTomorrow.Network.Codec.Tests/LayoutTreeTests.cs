using NexusTomorrow.Network.Codec.Tests.Facts;
using NexusTomorrow.Network.Codec.Tests.Fixtures;

namespace NexusTomorrow.Network.Codec.Tests;

public sealed class LayoutTreeTests
{
    private static WireLayout LayoutOf<T>()
        where T : IWireMessage
    {
        return T.Layout;
    }

    [Fact]
    public void AFixedMessageIsItsFields()
    {
        Assert.Equal("32 16 32 1", LayoutOf<Client009AFixture>().TreeText);
    }

    [Fact]
    public void EachStringIsOneNode()
    {
        Assert.Equal("w n t", LayoutOf<StringsFixture>().TreeText);
    }

    [Fact]
    public void AnArrayNamesItsCountAndNestsItsElement()
    {
        Assert.Equal("3 [0:18] 4 [2:32] [2:32] 8 8 8 2 [8:8 8]", LayoutOf<CollectionsFixture>().TreeText);
    }

    [Fact]
    public void AUnionListsEachArmBySelectorValue()
    {
        Assert.Equal("14 64 w 5 [3:4 16 16 {0:0=(1),2=(18)}] 16", LayoutOf<UnionArrayFixture>().TreeText);
    }

    [Fact]
    public void ABlobCarriesItsLengthWidthAndAlignment()
    {
        Assert.Equal("5 b32a", LayoutOf<EnvelopeFixture>().TreeText);
    }

    [Fact]
    public void ABaseComesFirstAndAPadIsAField()
    {
        Assert.Equal("8 12 4", LayoutOf<DerivedFixture>().TreeText);
    }

    // Client0592 nests NetworkGuid (32, 16, 16, then eight 8-bit fields) from the Network assembly, so its tree comes through [WireLayoutData].
    [Fact]
    public void AStructFromAnotherAssemblyNestsThroughItsLayoutData()
    {
        WireLayout layout = global::NexusTomorrow.Network.Auth.WireMessages.Layouts.Single(candidate => candidate.Opcode == 0x0592);

        Assert.StartsWith("32 64 t 32 16 16 8 8 8 8 8 8 8 8 32 16 16 8 8 8 8 8 8 8 8 32 32 32 32 w w w", layout.TreeText, StringComparison.Ordinal);
    }

    [Fact]
    public void EveryDeclaredTreeGivesItsEmptyPath()
    {
        WireLayout[] layouts =
        [
            .. global::NexusTomorrow.Network.WireMessages.Layouts,
            .. global::NexusTomorrow.Network.Auth.WireMessages.Layouts,
            .. global::NexusTomorrow.Network.World.WireMessages.Layouts,
        ];

        var problems = new List<string>();
        foreach (WireLayout layout in layouts.Where(candidate => candidate.IsComplete))
        {
            List<int>? widths = TreeWidths.EmptyPath(layout.Tree, readerView: false);
            if (widths is null || !widths.SequenceEqual(layout.EmptyPathWidths.Select(width => (int)width)))
                problems.Add($"0x{layout.Opcode:X4}: the tree gives [{(widths is null ? "none" : string.Join(", ", widths))}], the layout [{string.Join(", ", layout.EmptyPathWidths)}]");
        }

        Assert.Empty(problems);
    }
}
