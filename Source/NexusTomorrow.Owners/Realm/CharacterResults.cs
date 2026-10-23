namespace NexusTomorrow.Owners.Realm;

/// <summary>
/// <c>CharacterCreateResult.Result</c> values (0x00DC), as its declaration documents them.
/// </summary>
public static class CreateResults
{
    /// <summary>The character was created.</summary>
    public const byte Created = 3;

    /// <summary>The name is taken.</summary>
    public const byte NameTaken = 6;

    /// <summary>The account is full.</summary>
    public const byte AccountFull = 9;

    /// <summary>The name is not allowed.</summary>
    public const byte NameNotAllowed = 10;

    /// <summary>A value with no message of its own: the interface shows its general one.</summary>
    public const byte General = 1;
}

/// <summary>
/// <c>CharacterDeleteResult.Result</c> values (0x00E6), as its declaration documents them.
/// </summary>
public static class DeleteResults
{
    /// <summary>The character was deleted.</summary>
    public const byte Deleted = 0;

    /// <summary>A value with no message of its own: the interface shows its general one.</summary>
    public const byte General = 1;
}

/// <summary>
/// <c>CharacterRenameResult.Result</c> values (0x014B), as its declaration documents them.
/// </summary>
public static class RenameResults
{
    /// <summary>The character was renamed.</summary>
    public const byte Renamed = 18;

    /// <summary>An internal error.</summary>
    public const byte InternalError = 14;

    /// <summary>The name is not allowed.</summary>
    public const byte NameNotAllowed = 15;

    /// <summary>The name is taken.</summary>
    public const byte NameTaken = 16;

    /// <summary>No rename is available for the character.</summary>
    public const byte NoRenameAvailable = 17;
}

/// <summary>
/// <c>CharacterSelectFailed.Reason</c> values (0x0162), as its declaration documents them.
/// </summary>
public static class SelectReasons
{
    /// <summary>The character must be renamed first.</summary>
    public const byte MustRename = 1;

    /// <summary>A general failure: the character could not be loaded, or no map hosts its world.</summary>
    public const byte General = 2;

    /// <summary>The character is disabled.</summary>
    public const byte Disabled = 3;
}
