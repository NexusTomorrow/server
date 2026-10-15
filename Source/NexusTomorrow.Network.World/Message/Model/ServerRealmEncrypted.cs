using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerRealmEncrypted, Direction.ServerToClient)]
public sealed partial class ServerRealmEncrypted
{
    [Blob(32, LengthIncludesSelf = true)] public byte[] Data { get; set; } = [];
}
