using NexusTomorrow.Game.Static.Group;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server042D, Direction.ServerToClient)]
public sealed partial class Server042D
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(4)] public byte Unknown1 { get; set; }
}
