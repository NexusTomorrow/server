using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.Server0164, Direction.ServerToClient)]
public sealed partial class Server0164
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(2)] public byte Unknown1 { get; set; }
    [UInt(11)] public ushort Unknown2 { get; set; }
    [UInt(2)] public byte Unknown3 { get; set; }
    [UInt(8)] public byte Unknown4 { get; set; }
    [UInt(8)] public byte Unknown5 { get; set; }
    [UInt(8)] public byte Unknown6 { get; set; }
    [UInt(1)] public bool Unknown7 { get; set; }
    [UInt(8)] public byte Unknown8 { get; set; }
    [UInt(1)] public bool Unknown9 { get; set; }
    [UInt(1)] public bool Unknown10 { get; set; }
}
