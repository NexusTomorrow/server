using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Client00ED, Direction.ClientToServer)]
[Dormant]
public sealed partial class Client00ED
{
    [UInt(64)] public ulong Unknown0 { get; set; }
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
    [UInt(1)] public bool Unknown13 { get; set; }
    [UInt(1)] public bool Unknown14 { get; set; }
    [UInt(1)] public bool Unknown15 { get; set; }
}
