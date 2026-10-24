using NexusTomorrow.GameTable.Text.Static;
using NexusTomorrow.Owners.Text;

namespace NexusTomorrow.Owners.Tests;

public sealed class UserTextRulesTests
{
    [Fact]
    public void ATwoPartNameIsAllowed()
    {
        Assert.True(UserTextRules.Check("Jo Smith", UserText.CharacterName));
    }

    [Fact]
    public void ANameWithoutItsSpaceIsRefused()
    {
        Assert.False(UserTextRules.Check("Josmith", UserText.CharacterName));
    }

    [Theory]
    [InlineData("Jo  Smith")]
    [InlineData("Jo Sm Ith")]
    public void ANameWithTwoSpacesIsRefused(string name)
    {
        Assert.False(UserTextRules.Check(name, UserText.CharacterName));
    }

    [Theory]
    [InlineData(" Jo Smith")]
    [InlineData("Jo Smith ")]
    public void ANameWithAnEdgeSpaceIsRefused(string name)
    {
        Assert.False(UserTextRules.Check(name, UserText.CharacterName));
    }

    [Fact]
    public void ANameWithADigitIsRefused()
    {
        Assert.False(UserTextRules.Check("Jo Smith2", UserText.CharacterName));
    }

    [Fact]
    public void ANameOfFiveUnitsIsAllowedAndFourIsNot()
    {
        Assert.True(UserTextRules.Check("Jo Sm", UserText.CharacterName));
        Assert.False(UserTextRules.Check("J Sm", UserText.CharacterName));
    }

    [Fact]
    public void ANameOfThirtyUnitsIsAllowedAndThirtyOneIsNot()
    {
        string thirty = new string('A', 14) + " " + new string('B', 15);
        string thirtyOne = new string('A', 15) + " " + new string('B', 15);

        Assert.True(UserTextRules.Check(thirty, UserText.CharacterName));
        Assert.False(UserTextRules.Check(thirtyOne, UserText.CharacterName));
    }

    [Theory]
    [InlineData("Jø Smith", false)]
    [InlineData("Jé Smith", true)]
    public void ALetterOutsideTheClientsSetIsRefused(string name, bool allowed)
    {
        Assert.Equal(allowed, UserTextRules.Check(name, UserText.CharacterName));
    }

    [Fact]
    public void ANameMayBeLowerCase()
    {
        Assert.True(UserTextRules.Check("jo smith", UserText.CharacterName));
    }

    [Fact]
    public void AGuildNameMayHoldASpaceButNotAnUnderscore()
    {
        Assert.True(UserTextRules.Check("Blue Fox", UserText.GuildName));
        Assert.False(UserTextRules.Check("Blue_Fox", UserText.GuildName));
    }

    [Fact]
    public void AMailBodyMayBeEmptyAndMayBreakLines()
    {
        Assert.True(UserTextRules.Check("", UserText.MailBody));
        Assert.True(UserTextRules.Check("hi\nthere", UserText.MailBody));
    }

    [Fact]
    public void AnEmptyChatLineIsRefused()
    {
        Assert.False(UserTextRules.Check("", UserText.Chat));
    }

    [Fact]
    public void AnAllSpaceChatLineIsRefused()
    {
        Assert.False(UserTextRules.Check("   ", UserText.Chat));
    }

    [Fact]
    public void AChatLineOf499UnitsIsAllowedAnd500IsNot()
    {
        Assert.True(UserTextRules.Check(new string('a', 499), UserText.Chat));
        Assert.False(UserTextRules.Check(new string('a', 500), UserText.Chat));
    }

    [Fact]
    public void AChatLineWithALineBreakIsRefused()
    {
        Assert.False(UserTextRules.Check("hi\nthere", UserText.Chat));
    }

    [Theory]
    [InlineData(0x0001)]
    [InlineData(0x00A0)]
    public void AChatLineWithAControlOrNbspIsRefused(int unit)
    {
        Assert.False(UserTextRules.Check("a" + (char)unit, UserText.Chat));
    }

    [Theory]
    [InlineData(0x0000)]
    [InlineData(0xFFFE)]
    [InlineData(0xFFFF)]
    [InlineData(0x0A0D)]
    [InlineData(0x0D0A)]
    public void EachIllegalUnitIsRefused(int unit)
    {
        Assert.False(UserTextRules.Check("a" + (char)unit, UserText.Chat));
    }
}
