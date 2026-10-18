using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model;

/// <summary>
/// One already-encoded client message, unencrypted, sent only inside a <see cref="ClientEnvelope"/>.
/// </summary>
/// <remarks>
/// The client uses it only for character creation (0x025B).
/// </remarks>
[WireMessage(GameMessageOpcode.Client025C, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class NestedMessage
{
    /// <summary>
    /// One whole encoded message, which begins with its own 32-bit length (counting those four bytes).
    /// </summary>
    [Blob(32, LengthIncludesSelf = true)] public byte[] Payload { get; set; } = [];
}
