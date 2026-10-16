using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireMessage(GameMessageOpcode.Client059F, Direction.ClientToServer)]
public sealed partial class Client059F
{
    [UInt(9)] public ushort Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(9)] public ushort Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(9)] public ushort Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(9)] public ushort Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
}
