using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.Server0136, Direction.ServerToClient)]
public sealed partial class Server0136
{
    [UInt(14)] public uint Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
}
