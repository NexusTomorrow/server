using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.Server0516, Direction.ServerToClient)]
public sealed partial class Server0516
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(64)] public ulong Unknown1 { get; set; }
    [UInt(14)] public ushort Unknown2 { get; set; }
    [UInt(64)] public ulong Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(7)] public byte Unknown5 { get; set; }
}
