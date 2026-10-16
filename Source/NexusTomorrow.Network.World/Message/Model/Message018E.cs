using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Message018E, Direction.ServerToClient)]
public sealed partial class Message018E
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(18)] public uint Unknown2 { get; set; }
    [UInt(64)] public ulong Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(64)] public ulong Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    [UInt(64)] public ulong Unknown7 { get; set; }
    [UInt(18)] public uint Unknown8 { get; set; }
    [FixedArray(5), UInt(32)] public uint[] Unknown9 { get; set; } = new uint[5];
}
