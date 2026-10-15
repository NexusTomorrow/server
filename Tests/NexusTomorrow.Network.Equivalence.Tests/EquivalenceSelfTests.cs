using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.Network.Equivalence.Tests.SelfTest;
using NexusTomorrow.Network.Equivalence.Tests.SelfTest.Declared;

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

        Assert.NotEmpty(differences);
        Assert.All(differences, difference => Assert.Contains(" read: ReaderMessage.", difference, StringComparison.Ordinal));
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

    private static List<string> Check(Type declared)
    {
        using ServiceProvider legacyServices = new ServiceCollection().BuildServiceProvider();
        Type legacy = SelfTestTwins.Of(declared) ?? throw new InvalidOperationException($"{declared.Name} has no self-test twin.");
        return Equivalence.Check(declared, legacy, SelfTestTwins.Of, legacyServices, Seeds);
    }

    private static void AssertNone(List<string> differences)
    {
        if (differences.Count > 0)
            Assert.Fail(string.Join(Environment.NewLine, differences));
    }
}
