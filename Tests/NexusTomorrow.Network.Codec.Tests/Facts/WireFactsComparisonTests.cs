namespace NexusTomorrow.Network.Codec.Tests.Facts;

public sealed class WireFactsComparisonTests
{
    private static WireLayout Layout(Direction direction, bool isFixed, byte[] widths, params WirePart[] parts)
    {
        return new WireLayout(0x0198, direction, isFixed, isComplete: true, [.. widths], [.. parts], "");
    }

    private static WireFactsSide Fixed(params int[] widths)
    {
        return new WireFactsSide("fixed", widths.Sum(), widths, [], null);
    }

    [Fact]
    public void AMatchingLayoutReportsNothing()
    {
        WireFactsRow row = new(0x0198, "c2s", Fixed(32, 64), null);

        Assert.Empty(WireFactsComparison.Compare(Layout(Direction.ClientToServer, true, [32, 64]), row));
    }

    [Fact]
    public void FieldsInTheWrongOrderAreReportedThoughTheTotalMatches()
    {
        WireFactsRow row = new(0x0198, "c2s", Fixed(32, 64), null);

        List<string> problems = WireFactsComparison.Compare(Layout(Direction.ClientToServer, true, [64, 32]), row);

        Assert.Single(problems);
        Assert.Contains("[32, 64]", problems[0]);
    }

    [Fact]
    public void AFixedDeclarationOfAComputedMessageIsReported()
    {
        WireFactsRow row = new(0x0198, "c2s", new WireFactsSide("computed", 8, [1, 7], [new WireFactsPart("wide-string", null)], null), null);

        List<string> problems = WireFactsComparison.Compare(Layout(Direction.ClientToServer, true, [1, 7]), row);

        Assert.Contains(problems, problem => problem.Contains("computed", StringComparison.Ordinal));
        Assert.Contains(problems, problem => problem.Contains("wide-string", StringComparison.Ordinal));
    }

    [Fact]
    public void AReaderRowOmitsTheTerminatorOfAnEmptyTerminatedString()
    {
        WireFactsRow row = new(0x0198, "s2c", null, new WireFactsSide("computed", 16, [16], [], null));
        WireLayout layout = Layout(Direction.ServerToClient, false, [16, 16], new WirePart { Shape = WirePartShape.TerminatedWideString, At = 0, ElementBits = -1 });

        Assert.Empty(WireFactsComparison.Compare(layout, row));
    }

    [Fact]
    public void AnArrayWhoseElementSizeDiffersIsReported()
    {
        WireFactsRow row = new(0x0198, "c2s", new WireFactsSide("computed", 5, [5], [new WireFactsPart("array", 37)], null), null);
        WireLayout layout = Layout(Direction.ClientToServer, false, [5], new WirePart { Shape = WirePartShape.Array, At = 0, ElementBits = 36 });

        List<string> problems = WireFactsComparison.Compare(layout, row);

        Assert.Single(problems);
        Assert.Contains("37-bit", problems[0]);
    }

    [Fact]
    public void AnUnknownSideReportsNothing()
    {
        WireFactsRow row = new(0x0198, "s2c", null, new WireFactsSide("unknown", null, null, [], null));

        Assert.Empty(WireFactsComparison.Compare(Layout(Direction.ServerToClient, true, [8]), row));
    }

    [Fact]
    public void ADirectionTheClientDoesNotHandleIsReported()
    {
        WireFactsRow row = new(0x0198, "c2s", Fixed(8), null);

        List<string> problems = WireFactsComparison.Compare(Layout(Direction.Both, true, [8]), row);

        Assert.Single(problems);
        Assert.Contains("server to client", problems[0]);
    }

    [Fact]
    public void AnOpcodeWithoutARowIsReported()
    {
        Assert.Single(WireFactsComparison.Compare(Layout(Direction.ClientToServer, true, [8]), null));
    }

    [Fact]
    public void ASideWithATreeIsComparedForTheSameBitsOnEveryPath()
    {
        WireFactsRow row = new(0x0198, "c2s", new WireFactsSide("computed", 13, [8, 5], [], "8 5 [1:16 32]"), null);
        WireLayout layout = new(0x0198, Direction.ClientToServer, false, isComplete: true, [8, 5], [], "8 5 [1:16 16]");

        Assert.Equal(["0x0198 client to server [2].element[1]: the client has 32, the declaration 16"], WireFactsComparison.Compare(layout, row));
    }

    [Fact]
    public void AgreeingTreesReportNothing()
    {
        WireFactsRow row = new(0x0198, "s2c", null, new WireFactsSide("computed", 13, [8, 5], [], "8 5 [1:16 32]"));
        WireLayout layout = new(0x0198, Direction.ServerToClient, false, isComplete: true, [8, 5], [], "8 5 [1:16 32]");

        Assert.Empty(WireFactsComparison.Compare(layout, row));
    }

    [Fact]
    public void AnIncompleteLayoutIsReported()
    {
        WireFactsRow row = new(0x0198, "c2s", Fixed(8), null);
        WireLayout layout = new(0x0198, Direction.ClientToServer, false, isComplete: false, [8], [], "8");

        Assert.Contains(WireFactsComparison.Compare(layout, row), problem => problem.Contains("selector 0", StringComparison.Ordinal));
    }
}
