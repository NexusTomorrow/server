using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Codec.Tests.Facts;

public sealed class FieldsSplitsTests
{
    private const string Key = "0x0198 client to server [0]";
    private const string Split = "the client reads 14 as one field, the declaration as 8 6";

    private static WireLayout Layout(string tree, params byte[] widths)
    {
        return new WireLayout(0x0198, Direction.ClientToServer, true, isComplete: true, [.. widths], [], tree);
    }

    private static Dictionary<int, WireFactsRow> Rows(string tree, params int[] widths)
    {
        return new() { [0x0198] = new WireFactsRow(0x0198, "c2s", new WireFactsSide("fixed", widths.Sum(), widths, [], tree), null) };
    }

    private static Dictionary<string, Pin> Settled(params (string Key, Pin Pin)[] entries)
    {
        return entries.ToDictionary(entry => entry.Key, entry => entry.Pin, StringComparer.Ordinal);
    }

    [Fact]
    public void ARunWhereTheClientReadsOneFieldFailsAtFieldsUntilTheSplitIsSettled()
    {
        WireLayout split = Layout("8 6 4", 8, 6, 4);
        Dictionary<int, WireFactsRow> rows = Rows("14 4", 14, 4);

        Assert.Equal([$"split at Fields, not settled: {Key}: {Split}"], FieldsSplits.Check([(split, Rung.Fields, false)], rows, Settled()));
        Assert.Empty(FieldsSplits.Check([(split, Rung.Fields, false)], rows, Settled((Key, new Pin("settled: one value the client reads whole", Split)))));
    }

    [Fact]
    public void AMessageBelowFieldsMayKeepARun()
    {
        WireLayout split = Layout("8 6 4", 8, 6, 4);

        Assert.Empty(FieldsSplits.Check([(split, Rung.Purpose, false), (split, null, false)], Rows("14 4", 14, 4), Settled()));
    }

    [Fact]
    public void ALayoutConfirmedMessageBelowFieldsIsChecked()
    {
        WireLayout split = Layout("8 6 4", 8, 6, 4);

        Assert.Equal([$"split at Fields, not settled: {Key}: {Split}"], FieldsSplits.Check([(split, Rung.Purpose, true)], Rows("14 4", 14, 4), Settled()));
    }

    [Fact]
    public void ALayoutConfirmedMessageBelowFieldsTakesASettledSplit()
    {
        WireLayout split = Layout("8 6 4", 8, 6, 4);
        Dictionary<string, Pin> settled = Settled((Key, new Pin("settled: one value the client reads whole", Split)));

        Assert.Empty(FieldsSplits.Check([(split, Rung.Purpose, true)], Rows("14 4", 14, 4), settled));
    }

    [Fact]
    public void OneDeclaredFieldAgainstARunOfClientFieldsPassesAtFields()
    {
        Assert.Empty(FieldsSplits.Check([(Layout("32 4", 32, 4), Rung.Fields, false)], Rows("8 8 8 8 4", 8, 8, 8, 8, 4), Settled()));
    }

    [Fact]
    public void ASettledSplitThatChangedOrNoLongerOccursIsReported()
    {
        Dictionary<int, WireFactsRow> rows = Rows("14 4", 14, 4);
        Dictionary<string, Pin> settled = Settled((Key, new Pin("settled: one value the client reads whole", "the client reads 14 as one field, the declaration as 4 10")));

        Assert.Equal([$"settled, but the split changed: {Key}: {Split}"], FieldsSplits.Check([(Layout("8 6 4", 8, 6, 4), Rung.Fields, false)], rows, settled));
        Assert.Equal(
            [$"settled, but it is no longer split: {Key} (settled: one value the client reads whole)"],
            FieldsSplits.Check([(Layout("14 4", 14, 4), Rung.Fields, false)], rows, settled));
    }
}
