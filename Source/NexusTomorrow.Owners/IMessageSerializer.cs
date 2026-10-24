using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Owners;

/// <summary>
/// Serializes a server message once, behind the send gate, so an owner can send it to many sessions. Safe from any thread.
/// </summary>
public interface IMessageSerializer
{
    /// <summary>
    /// Run the send gate on <paramref name="message"/> as it is now, then serialize it. The result holds one reference, the caller's,
    /// which the caller releases after its sends; the message itself may be changed or reused at once. Throws
    /// <see cref="InvalidOperationException"/> when the send gate refuses the message, and <see cref="CodecException"/> when it writes
    /// a different number of bytes than it measured or holds more elements than a list's capacity.
    /// </summary>
    SerializedMessage Serialize<T>(T message)
        where T : class, IWireMessage;
}
