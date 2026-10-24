namespace NexusTomorrow.Owners.Text;

/// <summary>
/// Our names for the bits of the client's rule record for a kind of user text (<see cref="UserTextRules"/>), each applied per
/// UTF-16 unit.
/// </summary>
[Flags]
public enum UserTextFlags
{
    /// <summary>
    /// No rule beyond the lengths and the units every text is held to.
    /// </summary>
    None = 0,

    /// <summary>
    /// No two spaces in a row.
    /// </summary>
    NoDoubleSpace = 0x001,

    /// <summary>
    /// No space at the start or the end.
    /// </summary>
    NoEdgeSpace = 0x002,

    /// <summary>
    /// The first unit is a letter.
    /// </summary>
    StartsWithLetter = 0x004,

    /// <summary>
    /// The first letter is upper case and every later letter lower case.
    /// </summary>
    Capitalized = 0x008,

    /// <summary>
    /// No space at all.
    /// </summary>
    NoSpace = 0x010,

    /// <summary>
    /// Every unit but a space is a letter or a digit.
    /// </summary>
    LettersAndDigitsOnly = 0x020,

    /// <summary>
    /// A line break (<c>\n</c> or <c>\r</c>) is allowed; without this, either refuses the text.
    /// </summary>
    LineBreaks = 0x040,

    /// <summary>
    /// An empty or all-space text is allowed; without this, it is refused.
    /// </summary>
    EmptyAllowed = 0x080,

    /// <summary>
    /// Every unit but a space is a letter.
    /// </summary>
    LettersOnly = 0x100,

    /// <summary>
    /// An underscore passes <see cref="LettersAndDigitsOnly"/> and <see cref="LettersOnly"/>.
    /// </summary>
    Underscore = 0x200,

    /// <summary>
    /// Exactly one space: a second refuses the text, and so does none.
    /// </summary>
    OneSpace = 0x400,
}
