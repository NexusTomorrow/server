using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[WireMessage(GameMessageOpcode.Server074A, Direction.ServerToClient)]
public sealed partial class Server074A
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(16)] public ushort Unknown5 { get; set; }
}
