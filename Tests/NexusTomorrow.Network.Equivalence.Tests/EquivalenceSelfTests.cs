using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.Network.Equivalence.Tests.SelfTest;
using NexusTomorrow.Network.Equivalence.Tests.SelfTest.Declared;
using Legacy = NexusTomorrow.Network.Equivalence.Tests.SelfTest.Legacy;

namespace NexusTomorrow.Network.Equivalence.Tests;

public sealed class EquivalenceSelfTests
{
    private const int Seeds = 32;

    [Fact]
    public void AMatchingPairReportsNothing()
    {
        AssertNone(Check(typeof(MatchingMessage)));
    }

    [Fact]
    public void AWriterThatUsesADifferentWidthIsCaught()
    {
        List<string> differences = Check(typeof(WidthMessage));

        // The extra bit makes every legacy body a byte longer, so every instance differs.
        Assert.Equal(Seeds + 1, differences.Count);
        Assert.Contains("WidthMessage empty write: the bytes differ from bit 24: declared [000000], legacy [00000000]", differences);
        Assert.All(differences, difference => Assert.Contains(" write: the bytes differ from bit ", difference, StringComparison.Ordinal));
    }

    [Fact]
    public void AReaderThatDropsABitIsCaught()
    {
        List<string> differences = Check(typeof(ReaderMessage));

        Assert.Contains("ReaderMessage empty read: the legacy reader read 19 bits of the 20 written", differences);
        Assert.Contains(differences, difference => difference.Contains(" read: ReaderMessage.", StringComparison.Ordinal));
        Assert.All(differences, difference => Assert.Contains(" read: ", difference, StringComparison.Ordinal));
    }

    [Fact]
    public void EveryUnionArmIsVisited()
    {
        // The filler takes arm seed % 3 of the three arms in name order, so UnionArmC, the arm whose legacy writer is
        // a bit too wide, is the arm of seeds 2, 5, 8 and on.
        string[] thirdArmSeeds = [.. Enumerable.Range(0, Seeds).Where(seed => seed % 3 == 2).Select(seed => $"UnionMessage seed {seed} write: ")];

        List<string> differences = Check(typeof(UnionMessage));

        Assert.All(thirdArmSeeds, prefix => Assert.Contains(differences, difference => difference.StartsWith(prefix, StringComparison.Ordinal)));
        Assert.All(differences, difference => Assert.Contains(thirdArmSeeds, prefix => difference.StartsWith(prefix, StringComparison.Ordinal)));
    }

    [Fact]
    public void AWireMemberWithoutATwinIsReported()
    {
        List<string> differences = Check(typeof(OrphanMessage));

        Assert.Contains("OrphanMessage empty write: OrphanMessage.Extra has no legacy twin", differences);
        Assert.Contains("OrphanMessage empty read: OrphanMessage.Extra has no legacy twin", differences);
    }

    [Fact]
    public void ASharedCountAndAPadNeedNoTwin()
    {
        AssertNone(Check(typeof(CountPadMessage)));
    }

    [Fact]
    public void TheEmptyInstanceIsChecked()
    {
        string difference = Assert.Single(Check(typeof(EmptyMessage)));

        Assert.Equal("EmptyMessage empty write: the bytes differ from bit 32: declared [000000000000], legacy [00000000]", difference);
    }

    [Fact]
    public void AUnionSelectorTheLegacyArmComputesNeedsNoTwin()
    {
        AssertNone(Check(typeof(ComputedSelectorMessage)));
    }

    [Fact]
    public void AHolderOfOneUnionMapsOntoTheBareLegacyArm()
    {
        AssertNone(Check(typeof(ArmListMessage)));
    }

    [Fact]
    public void ADeclaredByteArrayStandsForALegacyBitArray()
    {
        AssertNone(Check(typeof(BitArrayMessage)));
    }

    [Fact]
    public void ABitArrayReadInAnotherByteOrderIsCaught()
    {
        List<string> differences = Check(typeof(ReversedBitArrayMessage));

        Assert.NotEmpty(differences);
        Assert.All(differences, difference => Assert.Contains(" read: ReversedBitArrayMessage.Flags[", difference, StringComparison.Ordinal));
    }

    [Fact]
    public void ALastFieldOneBitTooWideIsCaughtThoughTheBytesAgree()
    {
        List<string> differences = Check(typeof(TrailingWidthMessage));

        Assert.Contains("TrailingWidthMessage empty write: the legacy writer wrote 22 bits where the declaration measures 21", differences);
        Assert.Contains("TrailingWidthMessage empty read: the legacy reader read 22 bits of the 21 written", differences);
    }

    [Fact]
    public void AUnionArmWithoutATwinLandsOnTheHoldersFlatMembers()
    {
        AssertNone(Check(typeof(FlatUnionMessage)));
    }

    [Fact]
    public void AUnionArmWithATwinLandsInTheHoldersMemberForThatArm()
    {
        AssertNone(Check(typeof(SlotUnionMessage)));
    }

    [Fact]
    public void ColumnsPinnedAsRowsMatchTheLegacyRecords()
    {
        AssertNone(Check(typeof(RowsMessage), Pinned()));
    }

    [Fact]
    public void ColumnsWithoutTheirRowsRuleHaveNoTwin()
    {
        List<string> differences = Check(typeof(RowsMessage));

        Assert.Contains("RowsMessage empty write: RowsMessage.Ids has no legacy twin", differences);
        Assert.Contains("RowsMessage empty read: RowsMessage.Levels has no legacy twin", differences);
    }

    [Fact]
    public void ALimitAPresetAndARenameTogetherMatchTheLegacyQuirks()
    {
        AssertNone(Check(typeof(QuirkMessage), Pinned()));
    }

    [Fact]
    public void WithoutTheLimitAnIntegerHeldInAFloatIsCaught()
    {
        Assert.NotEmpty(Check(typeof(QuirkMessage), Pinned() with { Limits = new Dictionary<string, Rule>() }));
    }

    [Fact]
    public void WithoutThePresetTheLegacyEncodingIsCaught()
    {
        Assert.NotEmpty(Check(typeof(QuirkMessage), Pinned() with { Presets = new Dictionary<string, Rule>() }));
    }

    [Fact]
    public void WithoutTheRenameTheMemberHasNoTwin()
    {
        List<string> differences = Check(typeof(QuirkMessage), Pinned() with { Renames = new Dictionary<(string Type, string Member), Rename>() });

        Assert.Contains("QuirkMessage empty write: QuirkMessage.Total has no legacy twin", differences);
    }

    [Fact]
    public void AReadOnlyTypeIsHeldToTheLegacyReadAlone()
    {
        List<string> differences = Check(typeof(OneWayMessage));

        Assert.NotEmpty(differences);
        Assert.All(differences, difference => Assert.Contains(" write: the bytes differ from bit ", difference, StringComparison.Ordinal));
        AssertNone(Check(typeof(OneWayMessage), Pinned()));
    }

    [Fact]
    public void ASkippedArmIsNeverFilled()
    {
        // UnionArmC's legacy writer is a bit too wide; skipping it checks UnionMessage over its other two arms.
        AssertNone(Check(typeof(UnionMessage), new Pairing(SelfTestTwins.Of) { SkipsArm = arm => arm == typeof(UnionArmC) }));
    }

    [Fact]
    public void AnInstanceHoldingAReadOnlyArmIsCheckedOnReadAlone()
    {
        // UnionArmC's legacy writer is a bit too wide but its reader is right, so the instances holding it still have their selector read back.
        var pairing = new Pairing(SelfTestTwins.Of) { ReadOnly = new Dictionary<string, string> { [typeof(UnionArmC).FullName!] = "its legacy Write puts C in 7 bits" } };

        AssertNone(Check(typeof(UnionMessage), pairing));
    }

    [Fact]
    public void ALegacyReaderThatRunsPastTheBodyIsCaught()
    {
        List<string> differences = Check(typeof(OverrunMessage));

        Assert.Equal(Seeds + 1, differences.Count);
        Assert.All(differences, difference => Assert.Contains(" read: the legacy reader threw EndOfStreamException", difference, StringComparison.Ordinal));
    }

    [Fact]
    public void ALegacyReaderWithoutIReadableIsStillChecked()
    {
        List<string> differences = Check(typeof(InterfaceFreeMessage));

        Assert.Contains("InterfaceFreeMessage empty read: the legacy reader read 7 bits of the 8 written", differences);
    }

    [Fact]
    public void ALegacyTypeWithNeitherMethodChecksNothing()
    {
        Assert.False(Equivalence.CanCheck(typeof(Legacy.RowEntry)));
        Assert.True(Equivalence.CanCheck(typeof(Legacy.InterfaceFreeMessage)));
    }

    /// <summary>
    /// The rules the self-test twins need, one per deliberate legacy quirk.
    /// </summary>
    private static Pairing Pinned()
    {
        return new Pairing(SelfTestTwins.Of)
        {
            ReadOnly = new Dictionary<string, string> { [typeof(OneWayMessage).FullName!] = "its legacy Write puts Value in 9 bits" },
            Rows = new Dictionary<string, RowColumns> { [typeof(RowsMessage).FullName!] = new("Entries", [("Ids", "Id"), ("Levels", "Level")], "one record per entry") },
            Limits = new Dictionary<string, Rule> { [typeof(QuirkMessage).FullName!] = new("Amount is a float", value => ((QuirkMessage)value).Total &= (1u << 24) - 1) },
            Presets = new Dictionary<string, Rule> { [typeof(QuirkMessage).FullName!] = new("Mode picks the encoding", legacy => ((Legacy.QuirkMessage)legacy).Mode = Legacy.QuirkMode.Whole) },
            Renames = new Dictionary<(string Type, string Member), Rename> { [(typeof(QuirkMessage).FullName!, "Total")] = new("Amount", "the legacy name") },
        };
    }

    private static List<string> Check(Type declared, Pairing? pairing = null)
    {
        using ServiceProvider legacyServices = new ServiceCollection().BuildServiceProvider();
        Type legacy = SelfTestTwins.Of(declared) ?? throw new InvalidOperationException($"{declared.Name} has no self-test twin.");
        return Equivalence.Check(declared, legacy, pairing ?? new Pairing(SelfTestTwins.Of), legacyServices, Seeds);
    }

    private static void AssertNone(List<string> differences)
    {
        if (differences.Count > 0)
            Assert.Fail(string.Join(Environment.NewLine, differences));
    }
}
