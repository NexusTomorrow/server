using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.Server013A, Direction.ServerToClient)]
public sealed partial class Server013A
{
    [UInt(14)] public uint Unknown0 { get; set; }
    [UInt(14)] public uint Unknown1 { get; set; }
}
