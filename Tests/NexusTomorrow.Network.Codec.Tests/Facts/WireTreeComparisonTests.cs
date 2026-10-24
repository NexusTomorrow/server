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
    public void AClientFloatTheDeclarationReadsAsAnIntegerDisagrees()
    {
        Assert.Equal(["0xFFFF client to server [1]: the client reads a float, the declaration an integer"], Compare("8 32", "8 32f"));
    }

    [Fact]
    public void AClientFloatTheDeclarationReadsAsAFloatAgrees()
    {
        Assert.Empty(Compare("8 32f 64f", "8 32f 64f"));
    }

    [Fact]
    public void AnUnmarkedClientFieldTakesAnyEncoding()
    {
        Assert.Empty(Compare("32f 16h 8s 4", "32 16 8 4"));
    }

    [Fact]
    public void AnEncodingDisagreementDoesNotStopTheSequence()
    {
        Assert.Equal(
            [
                "0xFFFF client to server [0]: the client reads a float, the declaration a signed integer",
                "0xFFFF client to server [1].element[0]: the client reads a float, the declaration an integer",
                "0xFFFF client to server [2]: the client has 8, the declaration 4",
            ],
            Compare("32s [0:32] 4", "32f [0:32f] 8"));
    }

    [Fact]
    public void AShorterSequenceIsReported()
    {
        Assert.Equal(["0xFFFF client to server: the client's sequence goes on for 1 nodes after the declaration's ends"], Compare("8 16", "8 16 4"));
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
                "0xFFFF client to server [2].arm 0x00: the declaration's sequence goes on for 1 nodes after the client's ends",
                "0xFFFF client to server [2].arm 0x01: the declaration's sequence goes on for 1 nodes after the client's ends",
                "0xFFFF client to server [2].arm 0x02: the declaration's sequence goes on for 1 nodes after the client's ends",
                "0xFFFF client to server: the client's sequence goes on for 1 nodes after the declaration's ends",
            ],
            Compare("5 2 {1:0=(32),1=(32),2=(32 31)}", "5 2 {1:0=(),1=(),2=(32)} 32"));
    }

    [Fact]
    public void AnArmMissingTheSharedReadKeepsTheTailInEveryArm()
    {
        Assert.Equal(
            [
                "0xFFFF client to server [2].arm 0x00: the declaration's sequence goes on for 1 nodes after the client's ends",
                "0xFFFF client to server [2].arm 0x02: the declaration's sequence goes on for 1 nodes after the client's ends",
                "0xFFFF client to server: the client's sequence goes on for 1 nodes after the declaration's ends",
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
                "0xFFFF client to server [1].arm 0x00: the declaration's sequence goes on for 1 nodes after the client's ends",
                "0xFFFF client to server [1].arm 0x01: the declaration's sequence goes on for 1 nodes after the client's ends",
                "0xFFFF client to server: the client's sequence goes on for 1 nodes after the declaration's ends",
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
        Assert.Equal(["0xFFFF client to server [1].element: the declaration's sequence goes on for 1 nodes after the client's ends"], Compare("5 [0:16 16 16 16]", "5 [0:48]"));
    }

    [Fact]
    public void ARunOfMixedWidthsInAnElementEqualsOneFieldOfTheirTotal()
    {
        Assert.Empty(Compare("5 [0:16 32]", "5 [0:48]"));
    }

    [Fact]
    public void TwoEightsAtTheTopLevelEqualOneSixteen()
    {
        Assert.Empty(Compare("4 8 8 2", "4 16 2"));
        Assert.Empty(Compare("4 16 2", "4 8 8 2"));
    }

    [Fact]
    public void ARunOfMixedWidthsEqualsOneFieldOfTheirTotal()
    {
        Assert.Empty(Compare("3 16 8 32 1", "3 56 1"));
    }

    [Fact]
    public void TwoTwentyFoursEqualOneFortyEight()
    {
        Assert.Empty(Compare("24 24 w", "48 w"));
        Assert.Empty(Compare("48 w", "24 24 w"));
    }

    [Fact]
    public void ASplitAmongOtherFieldsOfAnElementAgrees()
    {
        Assert.Empty(Compare("5 [0:8 16 16 16 2]", "5 [0:8 48 2]"));
        Assert.Empty(Compare("5 [0:8 48 2]", "5 [0:8 16 32 2]"));
    }

    [Fact]
    public void AnElementCountedByOneBitSplitsLikeAnyOther()
    {
        Assert.Empty(Compare("1 [0:48]", "1 [0:16 16 16]"));
    }

    [Fact]
    public void AHoistedReadSplitsAfterTheUnion()
    {
        Assert.Empty(Compare("4 {0:0=(8 32),1=(16 32)}", "4 {0:0=(8),1=(16)} 16 16"));
    }

    [Fact]
    public void CountsAndSelectorsMapThroughTheAlignment()
    {
        Assert.Empty(Compare("16 4 [1:8] 3 {3:0=(),1=(8)}", "8 8 4 [2:8] 3 {4:0=(),1=(8)}"));
    }

    [Fact]
    public void ACountOrSelectorTheAlignmentMapsToAnotherNodeIsReported()
    {
        Assert.Equal(["0xFFFF client to server [3]: the client counts it with node 2, the declaration with node 1"], Compare("16 4 4 [1:8]", "8 8 4 4 [3:8]"));
        Assert.Equal(["0xFFFF client to server [3]: the client selects it with node 2, the declaration with node 1"], Compare("16 3 3 {1:0=(),1=(8)}", "8 8 3 3 {3:0=(),1=(8)}"));
    }

    [Theory]
    [InlineData("16 4 4 [2:8]", "8 8 4 4 [2:8]", "0xFFFF client to server [3]: the client counts it with node 1, the declaration with node 2")]
    [InlineData("16 3 3 {2:0=(),1=(8)}", "8 8 3 3 {2:0=(),1=(8)}", "0xFFFF client to server [3]: the client selects it with node 1, the declaration with node 2")]
    public void ACountOrSelectorWithTheSameRawIndexButAnotherAlignedNodeIsReported(string declared, string client, string problem)
    {
        Assert.Equal([problem], Compare(declared, client));
    }

    [Fact]
    public void TheComparisonReturnsTheSplitsItAccepts()
    {
        var splits = new List<WireSplit>();

        Assert.Empty(WireTreeComparison.Compare("0xFFFF client to server", WireTree.Parse("4 8 6 32 3 [4:8 8]"), WireTree.Parse("4 14 8 8 8 8 3 [6:16]"), splits));
        Assert.Equal(
            ["0xFFFF client to server [1]: 8 6 against 14", "0xFFFF client to server [3]: 32 against 8 8 8 8", "0xFFFF client to server [5].element[0]: 8 8 against 16"],
            splits.Select(split => $"{split.Key}: {string.Join(' ', split.Declared)} against {string.Join(' ', split.Client)}"));
    }

    [Fact]
    public void ARunOneSixtyFourBitSlotShortOrLongIsReported()
    {
        string client = "32 1 32 32 " + string.Join(' ', Enumerable.Repeat("8", 24));

        Assert.Empty(Compare("32 1 32 32 64 64 64", client));
        Assert.Equal(["0xFFFF client to server: the client's sequence goes on for 8 nodes after the declaration's ends"], Compare("32 1 32 32 64 64", client));
        Assert.Equal(["0xFFFF client to server: the declaration's sequence goes on for 1 nodes after the client's ends"], Compare("32 1 32 32 64 64 64 64", client));
    }

    [Fact]
    public void ATotalThatNeverMeetsIsReported()
    {
        Assert.Equal(["0xFFFF client to server [1]: the client has 24, the declaration 8"], Compare("5 8 8", "5 24"));
        Assert.Equal(["0xFFFF client to server [1]: the client has 8, the declaration 24"], Compare("5 24", "5 8 8"));
    }

    [Theory]
    [InlineData("4 4 [1:8]", "8 4 [1:8]", "0xFFFF client to server [0]: the client has 8, the declaration 4")]
    [InlineData("8 4 [1:8]", "4 4 [1:8]", "0xFFFF client to server [0]: the client has 4, the declaration 8")]
    [InlineData("4 4 [1:8]", "8 [0:8]", "0xFFFF client to server [0]: the client has 8, the declaration 4")]
    [InlineData("2 2 {1:0=(),1=(8)}", "4 {0:0=(),1=(8)}", "0xFFFF client to server [0]: the client has 4, the declaration 2")]
    public void ASplitNeverTakesInAFieldALaterNodeCountsOrSelectsWith(string declared, string client, string problem)
    {
        Assert.Equal([problem], Compare(declared, client));
    }

    [Theory]
    [InlineData("3 8 w 8", "3 16 w", "0xFFFF client to server [1]: the client has 16, the declaration 8")]
    [InlineData("3 16 w", "3 8 w 8", "0xFFFF client to server [1]: the client has 8, the declaration 16")]
    [InlineData("3 8 [0:4] 8", "3 16 [0:4]", "0xFFFF client to server [1]: the client has 16, the declaration 8")]
    [InlineData("3 8 {0:0=(),1=(4)} 8", "3 16 {0:0=(),1=(4)}", "0xFFFF client to server [1]: the client has 16, the declaration 8")]
    [InlineData("3 8 b8 8", "3 16 b8", "0xFFFF client to server [1]: the client has 16, the declaration 8")]
    public void ASplitNeverCrossesAStringAnArrayAUnionOrABlob(string declared, string client, string problem)
    {
        Assert.Equal([problem], Compare(declared, client));
    }

    [Theory]
    [InlineData("4 8 16", "4 16 8")]
    [InlineData("4 8 24", "4 16 16")]
    public void FieldsWhoseBoundariesCrossAreReportedThoughTheTotalsMeet(string declared, string client)
    {
        Assert.Equal(["0xFFFF client to server [1]: the client has 16, the declaration 8"], Compare(declared, client));
    }
}
