using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model;

[WireMessage(GameMessageOpcode.Message0003, Direction.ServerToClient)]
public sealed partial class Message0003
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(64)] public ulong Unknown4 { get; set; }
    [UInt(16)] public ushort Unknown5 { get; set; }
    [UInt(5)] public byte Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
    [UInt(32)] public uint Unknown8 { get; set; }
    [UInt(64)] public ulong Unknown9 { get; set; }
    [UInt(32)] public uint Unknown10 { get; set; }
}
