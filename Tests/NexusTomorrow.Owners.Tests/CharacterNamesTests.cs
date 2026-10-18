using NexusTomorrow.Owners.Realm;

namespace NexusTomorrow.Owners.Tests;

public sealed class CharacterNamesTests
{
    [Theory]
    [InlineData("Rebaser Head", true)]
    [InlineData("Ab Cd", true)]
    [InlineData("Abc", false)]                                  // no space
    [InlineData("Ab C", false)]                                 // four characters
    [InlineData(" Rebaser Head", false)]                        // leading space
    [InlineData("Rebaser Head ", false)]                        // trailing space
    [InlineData("Rebaser  Head", false)]                        // an empty part
    [InlineData("Rebaser H3ad", false)]                         // a digit
    [InlineData("Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa B", false)]      // a 30-character part (no 30-character part fits in 31)
    [InlineData("Aaaaaaaaaaaaaaa Bbbbbbbbbbbbbbbb", false)]      // 32 characters
    [InlineData("Aaaaaaaaaaaaaaa Bbbbbbbbbbbbbbb", true)]        // 31 characters
    [InlineData("Élodie Brun", true)]
    public void NamesFollowTheClientsLengthsAndOurLetterRule(string name, bool allowed)
    {
        Assert.Equal(allowed, CharacterNames.IsAllowed(name));
    }
}
