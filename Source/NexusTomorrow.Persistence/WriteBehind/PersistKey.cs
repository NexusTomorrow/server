namespace NexusTomorrow.Persistence;

/// <summary>
/// What a piece of persistence work touches. Work on the same key runs one at a time, in arrival order.
/// </summary>
public readonly record struct PersistKey(string Value)
{
    /// <summary>
    /// An account and everything stored under it.
    /// </summary>
    public static PersistKey Account(long id) => new($"account:{id}");

    /// <summary>
    /// One character.
    /// </summary>
    public static PersistKey Character(ulong id) => new($"character:{id}");

    /// <summary>
    /// An email address before its account exists, folded so case variants share a key. The database's collation decides
    /// uniqueness; the key only orders work.
    /// </summary>
    public static PersistKey Email(string email) => new($"email:{email.ToLowerInvariant()}");

    /// <summary>
    /// A named resource with no row of its own, such as an id sequence or the audit log.
    /// </summary>
    public static PersistKey Named(string name) => new($"named:{name}");
}
