using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03A0, Direction.ServerToClient)]
public sealed partial class Server03A0
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
}
