namespace NexusTomorrow.Network.Codec;

/// <summary>
/// The kinds of computed member a layout can hold, named as the client facts table names them.
/// </summary>
public enum WirePartShape
{
    /// <summary>
    /// A string with a 1-bit flag and a 7 or 15-bit length in UTF-16 code units.
    /// </summary>
    WideString = 1,

    /// <summary>
    /// A string with a 1-bit flag and a 7 or 15-bit length in bytes.
    /// </summary>
    NarrowString = 2,

    /// <summary>
    /// A string with a 16-bit count that includes its terminator.
    /// </summary>
    TerminatedWideString = 3,

    /// <summary>
    /// A counted array.
    /// </summary>
    Array = 4,

    /// <summary>
    /// A union whose arm a selector member chooses.
    /// </summary>
    Union = 5,

    /// <summary>
    /// A length-prefixed run of bytes.
    /// </summary>
    Blob = 6,
}
