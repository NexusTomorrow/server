using System.Buffers;
using NexusTomorrow.GameTable.Text.Static;
using static NexusTomorrow.Owners.Text.UserTextFlags;

namespace NexusTomorrow.Owners.Text;

/// <summary>
/// The client's rule for each kind of user text, its flags and its lengths, and the walk that applies them. The client checks a
/// text against its rule before sending it, and checks a character name's shape alone, so the server holds every text it acts on
/// to the same rule.
/// </summary>
public static class UserTextRules
{
    // The letters the client counts beyond A to Z and a to z. Ã Å Ð Õ Ø Þ ã å ð õ ø þ are not among them.
    private static readonly SearchValues<char> s_latinLetters = SearchValues.Create("ÀÁÂÄÆÇÈÉÊËÌÍÎÏÑÒÓÔÖÙÚÛÜÝßàáâäæçèéêëìíîïñòóôöùúûüýÿŒœŸ");

    // IsTextUnicode's IS_TEXT_UNICODE_ILLEGAL_CHARS set, which a valid text must not hold: UNICODE_NUL, a CR and an LF packed into
    // one unit (taken in either byte order), the reverse byte-order mark and 0xFFFF.
    // https://learn.microsoft.com/windows/win32/api/winbase/nf-winbase-istextunicode
    private static readonly SearchValues<char> s_illegal =
        SearchValues.Create([(char)0x0000, (char)0x0A0D, (char)0x0D0A, (char)0xFFFE, (char)0xFFFF]);

    /// <summary>
    /// The rule for <paramref name="type"/>: its flags, and its shortest and longest valid lengths in UTF-16 units.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="type"/> is not a kind of text the client registers.</exception>
    public static (UserTextFlags Flags, int MinLength, int MaxLength) For(UserText type) => type switch
    {
        UserText.CharacterName => (OneSpace | LettersOnly | StartsWithLetter | NoEdgeSpace, 5, 30),
        UserText.ScientistScanbotName => (LettersAndDigitsOnly | StartsWithLetter | NoEdgeSpace | NoDoubleSpace, 3, 16),
        UserText.GuildName => (LettersAndDigitsOnly | NoEdgeSpace | NoDoubleSpace, 3, 25),
        UserText.GuildRankName => (NoEdgeSpace, 1, 16),
        UserText.GuildBankTabName => (NoEdgeSpace, 1, 20),
        UserText.HousingResidenceName => (LettersAndDigitsOnly | StartsWithLetter | NoEdgeSpace | NoDoubleSpace, 3, 24),
        UserText.Chat => (None, 1, 499),
        UserText.MailSubject => (None, 1, 49),
        UserText.MailBody => (EmptyAllowed | LineBreaks, 0, 499),
        UserText.ChatCustomChannelName => (LettersAndDigitsOnly | NoSpace, 1, 20),
        UserText.ReadyCheck => (EmptyAllowed | NoEdgeSpace, 0, 499),
        UserText.FriendshipNote => (EmptyAllowed, 0, 32),
        UserText.ChatCustomChannelPassword => (EmptyAllowed, 0, 20),
        UserText.GuildMessageOfTheDay => (EmptyAllowed | LineBreaks, 0, 200),
        UserText.GuildMemberNote => (EmptyAllowed, 0, 32),
        UserText.GuildRecruitDescription => (EmptyAllowed | LineBreaks, 0, 400),
        UserText.GuildInfoMessage => (EmptyAllowed | LineBreaks, 0, 400),
        UserText.FriendshipAccountName => (LettersAndDigitsOnly | NoSpace, 3, 32),
        UserText.FriendshipAccountPrivateNote => (EmptyAllowed, 0, 64),
        UserText.FriendshipAccountPublicNote => (EmptyAllowed, 0, 128),
        UserText.FriendshipAccountEmail => (NoSpace, 5, 250),
        UserText.FriendshipInviteNote => (EmptyAllowed, 0, 128),
        UserText.PlayerTicketText => (None, 1, 491),
        UserText.CharacterNamePart => (LettersOnly | NoSpace | StartsWithLetter | NoEdgeSpace, 1, 28),
        UserText.PlayerTicketSubject => (None, 1, 90),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "The client registers no rule for this kind of text."),
    };

    /// <summary>
    /// True when <paramref name="text"/> meets the rule for <paramref name="type"/>: its lengths, its flags, the client's
    /// character classes, and no unit of IsTextUnicode's illegal set.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="type"/> is not a kind of text the client registers.</exception>
    public static bool Check(ReadOnlySpan<char> text, UserText type)
    {
        (UserTextFlags flags, int minLength, int maxLength) = For(type);
        if (text.Length < minLength || text.Length > maxLength)
            return false;

        int spaces = 0;
        bool sawText = false;
        bool sawLetter = false;
        for (int i = 0; i < text.Length; i++)
        {
            char unit = text[i];
            if (unit == ' ')
            {
                spaces++;
                if (!SpaceAllowed(flags, text, i, spaces))
                    return false;
                continue;
            }

            sawText = true;
            bool letter = IsLetter(unit);
            if (i == 0 && !letter && flags.HasFlag(StartsWithLetter))
                return false;
            if (!letter && !OtherUnitAllowed(unit, flags))
                return false;
            if (letter && flags.HasFlag(Capitalized) && !CasedForItsPlace(unit, firstLetter: !sawLetter))
                return false;
            sawLetter |= letter;
        }

        if (flags.HasFlag(OneSpace) && spaces != 1)
            return false;
        if (!sawText && !flags.HasFlag(EmptyAllowed))
            return false;

        return !text.ContainsAny(s_illegal);
    }

    // A to Z, a to z, and the Latin letters above.
    internal static bool IsLetter(char unit) => unit is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z') || s_latinLetters.Contains(unit);

    internal static bool IsDigit(char unit) => unit is >= '0' and <= '9';

    // A space is ruled by the space flags alone: LettersOnly and LettersAndDigitsOnly judge every other unit, which is why GuildName
    // carries NoDoubleSpace and NoEdgeSpace beside LettersAndDigitsOnly.
    private static bool SpaceAllowed(UserTextFlags flags, ReadOnlySpan<char> text, int index, int spaces) =>
        !flags.HasFlag(NoSpace)
        && !(flags.HasFlag(OneSpace) && spaces > 1)
        && !(flags.HasFlag(NoEdgeSpace) && (index == 0 || index == text.Length - 1))
        && !(flags.HasFlag(NoDoubleSpace) && index > 0 && text[index - 1] == ' ')
        && !(flags.HasFlag(StartsWithLetter) && index == 0);

    // A unit that is neither a letter nor a space.
    private static bool OtherUnitAllowed(char unit, UserTextFlags flags)
    {
        bool underscore = unit == '_' && flags.HasFlag(Underscore);
        if (flags.HasFlag(LettersOnly) && !underscore)
            return false;
        if (IsDigit(unit))
            return true;
        if (flags.HasFlag(LettersAndDigitsOnly) && !underscore)
            return false;
        if (unit is '\n' or '\r')
            return flags.HasFlag(LineBreaks);

        return !IsBarred(unit);
    }

    // The units the client refuses in any text: the C0 controls, DEL, the C1 controls, NBSP, the soft hyphen, the line and paragraph
    // separators, the noncharacters U+FDD0 to U+FDEF, the byte-order mark, U+FFFE and U+FFFF.
    private static bool IsBarred(char unit) =>
        (int)unit is < 0x20 or 0x7F or (>= 0x80 and <= 0x9F) or 0xA0 or 0xAD or 0x2028 or 0x2029 or (>= 0xFDD0 and <= 0xFDEF) or 0xFEFF
            or 0xFFFE or 0xFFFF;

    // The client folds case with towlower: the first letter must fold to another unit, and every later letter to itself.
    private static bool CasedForItsPlace(char unit, bool firstLetter) =>
        firstLetter ? char.ToLowerInvariant(unit) != unit : char.ToLowerInvariant(unit) == unit;
}
