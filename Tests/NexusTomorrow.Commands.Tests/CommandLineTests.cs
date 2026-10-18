namespace NexusTomorrow.Commands.Tests;

public sealed class CommandLineTests
{
    [Theory]
    [InlineData("account create a@b.c pw", new[] { "account", "create", "a@b.c", "pw" })]
    [InlineData("account create a@b.c \"correct horse\"", new[] { "account", "create", "a@b.c", "correct horse" })]
    [InlineData("  server   status ", new[] { "server", "status" })]
    [InlineData("say \"\"", new[] { "say", "" })]
    public void TokensSplitOnSpacesAndQuotesGroup(string line, string[] expected)
    {
        Assert.True(CommandLine.TrySplit(line, out List<string> tokens));
        Assert.Equal(expected, tokens);
    }

    [Fact]
    public void AnUnclosedQuoteIsRefused()
    {
        Assert.False(CommandLine.TrySplit("account create a@b.c \"open", out _));
    }

    [Fact]
    public void ABlankLineHasNoTokens()
    {
        Assert.True(CommandLine.TrySplit("   ", out List<string> tokens));
        Assert.Empty(tokens);
    }
}
