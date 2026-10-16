using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0172, Direction.ServerToClient)]
public sealed partial class Server0172
{
    [UInt(8)] public uint Unknown0 { get; set; }
    [UInt(8)] public uint Unknown1 { get; set; }
}
