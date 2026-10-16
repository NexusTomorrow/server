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
    /// One whole message, which begins with its own 32-bit length (counting those four bytes). The length travels in clear and the bytes
    /// after it are encrypted with the login connection's key; the client decrypts them and handles the result as that message.
    /// </summary>
    [Blob(32, LengthIncludesSelf = true)] public byte[] Payload { get; set; } = [];
}
