using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Message0760, Direction.Both)]
public sealed partial class Message0760
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; } = "";
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(2)] public byte Unknown4 { get; set; }
    [UInt(3)] public byte Unknown5 { get; set; }
    [UInt(3)] public byte Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
    [UInt(8)] public byte Unknown8 { get; set; }
    [UInt(8)] public byte Unknown9 { get; set; }
    [UInt(8)] public byte Unknown10 { get; set; }
    [UInt(8)] public byte Unknown11 { get; set; }
    [UInt(8)] public byte Unknown12 { get; set; }
    [UInt(8)] public byte Unknown13 { get; set; }
    [UInt(8)] public byte Unknown14 { get; set; }
    [UInt(8)] public byte Unknown15 { get; set; }
    [UInt(8)] public byte Unknown16 { get; set; }
    [UInt(8)] public byte Unknown17 { get; set; }
    [UInt(8)] public byte Unknown18 { get; set; }
    [UInt(8)] public byte Unknown19 { get; set; }
    [UInt(8)] public byte Unknown20 { get; set; }
    [UInt(8)] public byte Unknown21 { get; set; }
    [UInt(8)] public byte Unknown22 { get; set; }
    [UInt(8)] public byte Unknown23 { get; set; }
    [UInt(14)] public ushort Unknown24 { get; set; }
    [UInt(32)] public uint Unknown25 { get; set; }
    [WideString] public string Unknown26 { get; set; } = "";
    [UInt(64)] public ulong Unknown27 { get; set; }
    [UInt(16)] public ushort Unknown28 { get; set; }
    [UInt(16)] public ushort Unknown29 { get; set; }
    [UInt(16)] public ushort Unknown30 { get; set; }
    [UInt(16)] public ushort Unknown31 { get; set; }
}
