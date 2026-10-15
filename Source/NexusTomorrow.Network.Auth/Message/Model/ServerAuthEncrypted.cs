using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

[WireMessage(GameMessageOpcode.ServerAuthEncrypted, Direction.ServerToClient)]
public sealed partial class ServerAuthEncrypted
{
    [Blob(32, LengthIncludesSelf = true)] public byte[] Data { get; set; } = [];
}
