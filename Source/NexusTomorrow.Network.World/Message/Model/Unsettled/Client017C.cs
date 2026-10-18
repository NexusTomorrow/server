using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Client017C, Direction.ClientToServer)]
public sealed partial class Client017C
{
    [UInt(8)] public byte Unknown0 { get; set; }
    [UInt(8)] public byte Unknown1 { get; set; }
    [UInt(8)] public byte Unknown2 { get; set; }
    [UInt(8)] public byte Unknown3 { get; set; }
    [UInt(8)] public byte Unknown4 { get; set; }
    [UInt(8)] public byte Unknown5 { get; set; }
    [UInt(8)] public byte Unknown6 { get; set; }
    [UInt(8)] public byte Unknown7 { get; set; }
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
}
