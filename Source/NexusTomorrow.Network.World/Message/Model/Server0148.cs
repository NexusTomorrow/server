using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0148, Direction.ServerToClient)]
public sealed partial class Server0148
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(6)] public byte Unknown1 { get; set; }
}
