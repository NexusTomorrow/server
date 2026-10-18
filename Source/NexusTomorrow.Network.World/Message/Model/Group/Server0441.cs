using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Group;

[WireMessage(GameMessageOpcode.Server0441, Direction.ServerToClient)]
public sealed partial class Server0441
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(14)] public ushort Unknown2 { get; set; }
    [UInt(64)] public ulong Unknown3 { get; set; }
    [UInt(14)] public ushort Unknown4 { get; set; }
    [UInt(64)] public ulong Unknown5 { get; set; }
    [UInt(1)] public bool Unknown6 { get; set; }
}
