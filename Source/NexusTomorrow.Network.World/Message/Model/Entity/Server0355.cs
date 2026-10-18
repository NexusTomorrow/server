using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Server0355, Direction.ServerToClient)]
public sealed partial class Server0355
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
}
