using System.Reflection;
using NexusTomorrow.GameTable.Text.Static;

namespace NexusTomorrow.Owners.Realm;

/// <summary>
/// The server's check of a new character name: the client's lengths for a whole name and for each part, at least one space, no
/// space at either end, and letters only.
/// </summary>
public static class CharacterNames
{
    private static readonly UserTextAttribute s_whole = Rule(UserText.CharacterName);
    private static readonly UserTextAttribute s_part = Rule(UserText.CharacterNamePart);

    /// <summary>
    /// True when <paramref name="name"/> may be used.
    /// </summary>
    public static bool IsAllowed(string name)
    {
        if (name.Length < s_whole.MinLength || name.Length > s_whole.MaxLength)
            return false;
        if (name[0] == ' ' || name[^1] == ' ')
            return false;

        string[] parts = name.Split(' ');
        if (parts.Length < 2)
            return false;
        foreach (string part in parts)
        {
            if (part.Length < s_part.MinLength || part.Length > s_part.MaxLength)
                return false;
            foreach (char character in part)
            {
                if (!char.IsLetter(character))
                    return false;
            }
        }

        return true;
    }

    private static UserTextAttribute Rule(UserText text) =>
        typeof(UserText).GetField(text.ToString())?.GetCustomAttribute<UserTextAttribute>()
        ?? throw new InvalidOperationException($"UserText.{text} carries no UserTextAttribute.");
}
