using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Server037F, Direction.ServerToClient)]
public sealed partial class Server037F
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(1)] public bool Unknown4 { get; set; }
    [UInt(1)] public bool Unknown5 { get; set; }
}
