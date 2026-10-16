using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03CB, Direction.ServerToClient)]
public sealed partial class Server03CB
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(4)] public byte Unknown1 { get; set; }
}
