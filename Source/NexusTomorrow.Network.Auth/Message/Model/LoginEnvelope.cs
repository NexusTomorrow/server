using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

/// <summary>
/// A server message encrypted with the login connection's key.
/// </summary>
/// <remarks>
/// The client picks the key by opcode, not by the connection the envelope arrives on. Every server message but a
/// <see cref="Network.Message.Model.ServerHello"/> on the login connection arrives inside this or <c>WorldEnvelope</c> (0x03DC); the client
/// drops any other message that arrives bare.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0076, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class LoginEnvelope
{
    /// <summary>
    /// A 32-bit length that counts its own four bytes, then the encrypted message; the length must be at least 4. The client decrypts the
    /// bytes after the length and handles them as one whole message.
    /// </summary>
    [Blob(32, LengthIncludesSelf = true)] public byte[] Payload { get; set; } = [];
}
