namespace NexusTomorrow.Network.Codec.Tests.Facts;

public sealed class WireTreeComparisonTests
{
    private static List<string> Compare(string declared, string client)
    {
        return WireTreeComparison.Compare("0xFFFF client to server", WireTree.Parse(declared), WireTree.Parse(client));
    }

    [Fact]
    public void EqualTreesAgree()
    {
        Assert.Empty(Compare("14 64 w 5 [3:4 16 16 {0:0=(1),2=(18)}] 16", "14 64 w 5 [3:4 16 16 {0:0=(1),2=(18)}] 16"));
    }

    [Fact]
    public void AWidthDifferenceIsReportedAtItsNodeAndStopsTheSequence()
    {
        Assert.Equal(["0xFFFF client to server [1]: the client has 32, the declaration 16"], Compare("8 16 4", "8 32 5"));
    }

    [Fact]
    public void AShorterSequenceIsReported()
    {
        Assert.Equal(["0xFFFF client to server: the client's sequence has 3 nodes, the declaration's 2"], Compare("8 16", "8 16 4"));
    }

    [Fact]
    public void ArrayElementsCompareUnderTheirPath()
    {
        Assert.Equal(["0xFFFF client to server [1].element[1]: the client has 16, the declaration 8"], Compare("5 [0:8 8]", "5 [0:8 16]"));
    }

    [Fact]
    public void ArmsCompareBySelectorValue()
    {
        Assert.Equal(
            [
                "0xFFFF client to server [1]: the declaration has an arm for selector 1, which the client does not accept",
                "0xFFFF client to server [1].arm 0x02[0]: the client has 17, the declaration 18",
                "0xFFFF client to server [1]: the client accepts selector 3 and the declaration has no arm",
            ],
            Compare("4 {0:0=(),1=(1),2=(18)}", "4 {0:0=(),2=(17),3=(5)}"));
    }

    [Fact]
    public void EmptyClientOnlyArmsCollapse()
    {
        Assert.Equal(
            ["0xFFFF client to server [1]: the client reads selectors 2 to 4 with nothing after them, and the declaration has no arm for them"],
            Compare("3 {0:0=(),1=(8)}", "3 {0:0=(),1=(8),2=(),3=(),4=()}"));
    }

    [Fact]
    public void ValuesAWideSelectorWasNeverTriedAtAreNotCompared()
    {
        Assert.Empty(Compare("12 {0:0=(),300=(8)}", "12 {0:0=()}"));
    }

    [Fact]
    public void AOneBitCountedArrayEqualsTheUnionTheProbeReports()
    {
        Assert.Empty(Compare("1 [0:8 8]", "1 {0:0=(),1=(8 8)}"));
    }

    [Fact]
    public void ArraysOfOneWidthSharingACountEqualOneArray()
    {
        Assert.Empty(Compare("4 [0:32] [0:32] 8 [3:8]", "4 [0:32 32] 8 [2:8]"));
    }
}
