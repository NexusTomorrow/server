using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireStruct]
public sealed partial class Server07CDPart0
{
    [UInt(14)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [FixedArray(4), UInt(8)] public byte[] Unknown2 { get; set; } = new byte[4];
    [UInt(32), Count] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [Array(CountFrom = nameof(Unknown3)), UInt(32)] public List<uint> Unknown5 { get; set; } = [];
    [UInt(1)] public bool Unknown6 { get; set; }
}
