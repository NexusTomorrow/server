using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.Server06FD, Direction.ServerToClient)]
public sealed partial class Server06FD
{
    [UInt(14)] public uint Unknown0 { get; set; }
    [UInt(14)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
}
