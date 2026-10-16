using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0905, Direction.ServerToClient)]
public sealed partial class Server0905
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(5)] public byte Unknown1 { get; set; }
    [UInt(2)] public byte Unknown2 { get; set; }
    [UInt(18)] public uint Unknown3 { get; set; }
    [UInt(17)] public uint Unknown4 { get; set; }
    [UInt(15)] public ushort Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    [UInt(1)] public bool Unknown7 { get; set; }

    [Array(CountBits = 32)] public List<Server0117Part1> Unknown8 { get; set; } = [];
}
