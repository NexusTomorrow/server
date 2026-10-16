using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server093C, Direction.ServerToClient)]
public sealed partial class Server093C
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(4)] public byte Unknown1 { get; set; }
    [UInt(14)] public uint Unknown2 { get; set; }
}
