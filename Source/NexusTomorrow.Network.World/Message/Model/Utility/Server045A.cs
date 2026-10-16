using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.Server045A, Direction.ServerToClient)]
public sealed partial class Server045A
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
}
