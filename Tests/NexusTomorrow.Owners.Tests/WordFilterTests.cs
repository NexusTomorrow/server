using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NexusTomorrow.Owners.Realm;
using NexusTomorrow.Owners.Text;

namespace NexusTomorrow.Owners.Tests;

// Every row here is a made-up word, never one of the client's.
public sealed class WordFilterTests
{
    [Theory]
    [InlineData("say blorp now")]
    [InlineData("BLORP")]
    [InlineData("BlOrP")]
    public void AnExactRowFiltersTheWordInAnyCase(string text)
    {
        Assert.False(Filter((0, "blorp")).Allows(text));
    }

    [Theory]
    [InlineData("blorps")]
    [InlineData("xblorp")]
    [InlineData("blorp9")]
    public void AnExactRowLeavesALongerWord(string text)
    {
        Assert.True(Filter((0, "blorp")).Allows(text));
    }

    [Fact]
    public void APrefixRowFiltersAWordThatStartsWithIt()
    {
        WordFilter filter = Filter((2, "zzq"));

        Assert.False(filter.Allows("zzqa"));
        Assert.True(filter.Allows("azzq"));
    }

    [Fact]
    public void ASuffixRowFiltersAWordThatEndsWithIt()
    {
        WordFilter filter = Filter((3, "qqz"));

        Assert.False(filter.Allows("aqqz"));
        Assert.True(filter.Allows("qqza"));
    }

    [Fact]
    public void AnAnywhereRowFiltersAWordThatHoldsIt()
    {
        Assert.False(Filter((4, "mmk")).Allows("ammkb"));
    }

    [Fact]
    public void ARawPatternRowFiltersAWordItMatches()
    {
        WordFilter filter = Filter((1, "^bl[aeiou]rp$"));

        Assert.False(filter.Allows("say BLARP"));
        Assert.True(filter.Allows("blrp"));
    }

    [Fact]
    public void ARawPatternThatDoesNotCompileIsSkipped()
    {
        var logger = new RecordingLogger<WordFilter>();

        var filter = new WordFilter([(1, "("), (0, "blorp")], logger);

        Assert.False(filter.Allows("blorp"));
        Assert.StartsWith(nameof(LogLevel.Warning), Assert.Single(logger.Entries));
    }

    [Fact]
    public void NoTableAllowsEverything()
    {
        var logger = new RecordingLogger<TableWordFilter>();

        var filter = new TableWordFilter(Options.Create(new ContentOptions { ClientDataPath = "" }), logger);

        Assert.True(filter.Allows("blorp"));
        Assert.StartsWith(nameof(LogLevel.Warning), Assert.Single(logger.Entries));
    }

    private static WordFilter Filter((uint Type, string Text) row) => new([row], NullLogger<WordFilter>.Instance);
}
