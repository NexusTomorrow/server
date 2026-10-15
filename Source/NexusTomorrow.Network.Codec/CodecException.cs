namespace NexusTomorrow.Network.Codec;

/// <summary>
/// A message could not be read or written: the body ended early, a value or count does not fit its declared width, or a union selector names no arm.
/// </summary>
public sealed class CodecException : Exception
{
    /// <summary>
    /// Create a <see cref="CodecException"/> with the supplied message.
    /// </summary>
    public CodecException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Create a <see cref="CodecException"/> with the supplied message and inner exception.
    /// </summary>
    public CodecException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
