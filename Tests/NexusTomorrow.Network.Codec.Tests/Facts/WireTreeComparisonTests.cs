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
    public void ValuesAWideSelectorHasNoFactsForAreNotCompared()
    {
        Assert.Empty(Compare("12 {0:0=(),300=(8)}", "12 {0:0=()}"));
    }

    [Fact]
    public void AOneBitCountedArrayEqualsItsUnionForm()
    {
        Assert.Empty(Compare("1 [0:8 8]", "1 {0:0=(),1=(8 8)}"));
    }

    [Fact]
    public void ArraysOfOneWidthSharingACountEqualOneArray()
    {
        Assert.Empty(Compare("4 [0:32] [0:32] 8 [3:8]", "4 [0:32 32] 8 [2:8]"));
    }

    [Fact]
    public void AReadEveryArmEndsWithEqualsTheSameReadAfterTheUnion()
    {
        Assert.Empty(Compare("8 5 [1:5 2 {1:0=(32),1=(32),2=(32 32)}]", "8 5 [1:5 2 {1:0=(),1=(),2=(32)} 32]"));
    }

    [Fact]
    public void ATailOfSeveralReadsEveryArmEndsWithLeavesTheUnionWhole()
    {
        Assert.Empty(Compare("8 [0:3 {0:0=(18 32 n),1=(w 32 n),5=(32 32 n)}]", "8 [0:3 {0:0=(18),1=(w),5=(32)} 32 n]"));
    }

    [Fact]
    public void ASharedTailLeavesTheUnionOnEitherSide()
    {
        Assert.Empty(Compare("4 {0:0=(8),1=(16)} 32", "4 {0:0=(8 32),1=(16 32)}"));
    }

    [Fact]
    public void ACountAfterAHoistedReadStillNamesItsField()
    {
        Assert.Empty(Compare("4 {0:0=(8 32),1=(16 32)} 3 [2:8]", "4 {0:0=(8),1=(16)} 32 3 [3:8]"));
    }

    [Fact]
    public void AnArmEndingInAnotherWidthKeepsTheTailInEveryArm()
    {
        Assert.Equal(
            [
                "0xFFFF client to server [2].arm 0x00: the client's sequence has 0 nodes, the declaration's 1",
                "0xFFFF client to server [2].arm 0x01: the client's sequence has 0 nodes, the declaration's 1",
                "0xFFFF client to server [2].arm 0x02: the client's sequence has 1 nodes, the declaration's 2",
                "0xFFFF client to server: the client's sequence has 4 nodes, the declaration's 3",
            ],
            Compare("5 2 {1:0=(32),1=(32),2=(32 31)}", "5 2 {1:0=(),1=(),2=(32)} 32"));
    }

    [Fact]
    public void AnArmMissingTheSharedReadKeepsTheTailInEveryArm()
    {
        Assert.Equal(
            [
                "0xFFFF client to server [2].arm 0x00: the client's sequence has 0 nodes, the declaration's 1",
                "0xFFFF client to server [2].arm 0x02: the client's sequence has 1 nodes, the declaration's 2",
                "0xFFFF client to server: the client's sequence has 4 nodes, the declaration's 3",
            ],
            Compare("5 2 {1:0=(32),1=(),2=(32 32)}", "5 2 {1:0=(),1=(),2=(32)} 32"));
    }

    [Fact]
    public void AHoistedReadOfAnotherWidthIsReportedAfterTheUnion()
    {
        Assert.Equal(["0xFFFF client to server [2]: the client has 32, the declaration 31"], Compare("4 {0:0=(8 31),1=(16 31)}", "4 {0:0=(8),1=(16)} 32"));
    }

    [Fact]
    public void AWidthDifferenceBeforeTheSharedTailIsReportedInItsArm()
    {
        Assert.Equal(["0xFFFF client to server [1].arm 0x01[0]: the client has 15, the declaration 16"], Compare("4 {0:0=(8 32),1=(16 32)}", "4 {0:0=(8),1=(15)} 32"));
    }

    [Fact]
    public void ArmsEndingInStringsOfTwoKindsShareNoTail()
    {
        Assert.Equal(
            [
                "0xFFFF client to server [1].arm 0x00: the client's sequence has 1 nodes, the declaration's 2",
                "0xFFFF client to server [1].arm 0x01: the client's sequence has 1 nodes, the declaration's 2",
                "0xFFFF client to server: the client's sequence has 3 nodes, the declaration's 2",
            ],
            Compare("4 {0:0=(8 n),1=(16 w)}", "4 {0:0=(8),1=(16)} n"));
    }

    [Fact]
    public void AnArrayAtTheEndOfEveryArmStaysInItsArm()
    {
        Assert.Equal(["0xFFFF client to server [1].arm 0x01[1].element[0]: the client has 8, the declaration 16"], Compare("4 {0:0=(3 [0:8]),1=(3 [0:16])}", "4 {0:0=(3 [0:8]),1=(3 [0:8])}"));
    }

    [Fact]
    public void AOneFieldElementEqualsARunOfOneWidthWithTheSameTotal()
    {
        Assert.Empty(Compare("10 [0:32] [0:16 16 16] 2 32 1", "10 [0:32] [0:48] 2 32 1"));
    }

    [Fact]
    public void TheOneFieldElementMayBeTheDeclaration()
    {
        Assert.Empty(Compare("5 [0:48]", "5 [0:16 16 16]"));
    }

    [Fact]
    public void ARunOfTwoFieldsEqualsOneFieldOfTheirTotal()
    {
        Assert.Empty(Compare("5 [0:24 24]", "5 [0:48]"));
    }

    [Fact]
    public void ARunOfMixedWidthsOneBitShortIsReported()
    {
        Assert.Equal(["0xFFFF client to server [1].element[0]: the client has 48, the declaration 16"], Compare("5 [0:16 16 15]", "5 [0:48]"));
    }

    [Fact]
    public void ARunOfOneWidthWithAnotherTotalIsReported()
    {
        Assert.Equal(["0xFFFF client to server [1].element[0]: the client has 48, the declaration 16"], Compare("5 [0:16 16 16 16]", "5 [0:48]"));
    }

    [Fact]
    public void ARunOfMixedWidthsIsComparedFieldByField()
    {
        Assert.Equal(["0xFFFF client to server [1].element[0]: the client has 48, the declaration 16"], Compare("5 [0:16 32]", "5 [0:48]"));
    }
}
