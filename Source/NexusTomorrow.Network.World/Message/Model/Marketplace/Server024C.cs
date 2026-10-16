using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server024C, Direction.ServerToClient)]
public sealed partial class Server024C
{
    [UInt(18)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [FixedArray(3), UInt(64)] public ulong[] Unknown2 { get; set; } = new ulong[3];
    [UInt(32)] public uint Unknown3 { get; set; }
    [FixedArray(3), UInt(64)] public ulong[] Unknown4 { get; set; } = new ulong[3];
    [Array(CountBits = 32)] public List<Server024APart0> Unknown5 { get; set; } = [];
}
