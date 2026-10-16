using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.Server0112, Direction.ServerToClient)]
public sealed partial class Server0112
{
    [UInt(14)] public uint Unknown0 { get; set; }
    [UInt(14)] public ushort Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(1)] public bool Unknown3 { get; set; }
    [Array(CountBits = 32)] public List<Server0112Part0> Unknown4 { get; set; } = [];
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown5 { get; set; } = [];
    [UInt(32)] public uint Unknown6 { get; set; }
    [FixedArray(3), UInt(32)] public uint[] Unknown7 { get; set; } = new uint[3];
    [UInt(14)] public uint Unknown8 { get; set; }
}
