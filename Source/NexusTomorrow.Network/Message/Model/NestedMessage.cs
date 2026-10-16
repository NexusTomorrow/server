using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model;

/// <summary>
/// One already-encoded client message, unencrypted, sent only inside a <see cref="ClientEnvelope"/>.
/// </summary>
/// <remarks>
/// The client uses it only for character creation, around <c>Client025B</c> (0x025B).
/// </remarks>
[WireMessage(GameMessageOpcode.Client025C, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class NestedMessage
{
    /// <summary>
    /// A 32-bit length that counts its own four bytes, then the encoded message.
    /// </summary>
    [Blob(32, LengthIncludesSelf = true)] public byte[] Payload { get; private set; } = [];
}
