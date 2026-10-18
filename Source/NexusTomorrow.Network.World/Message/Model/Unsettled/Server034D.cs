using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server034D, Direction.ServerToClient)]
public sealed partial class Server034D
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
    [UInt(32)] public uint Unknown8 { get; set; }
    [UInt(32)] public uint Unknown9 { get; set; }
    [UInt(32)] public uint Unknown10 { get; set; }
    [UInt(32)] public uint Unknown11 { get; set; }
    [UInt(32)] public uint Unknown12 { get; set; }
    [UInt(32)] public uint Unknown13 { get; set; }
    [UInt(32)] public uint Unknown14 { get; set; }
    [UInt(32)] public uint Unknown15 { get; set; }
}
