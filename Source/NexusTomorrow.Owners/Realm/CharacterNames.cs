using NexusTomorrow.GameTable.Text.Static;
using NexusTomorrow.Owners.Text;

namespace NexusTomorrow.Owners.Realm;

/// <summary>
/// The server's check of a new character name: the client's rule for a whole name (two parts of the client's letters joined by one
/// space, 5 to 30 units, <see cref="UserTextRules"/>), then the word filter, which the client never runs on a name.
/// </summary>
public static class CharacterNames
{
    /// <summary>
    /// True when <paramref name="name"/> meets the client's name rule and <paramref name="filter"/> allows it.
    /// </summary>
    public static bool IsAllowed(string name, IWordFilter filter) =>
        UserTextRules.Check(name, UserText.CharacterName) && filter.Allows(name);
}
