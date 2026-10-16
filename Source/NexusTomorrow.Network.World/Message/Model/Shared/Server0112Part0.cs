using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0112Part0
{
    [UInt(15)] public uint Unknown0 { get; set; }
    public Server0112Part1 Unknown1 { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown5 { get; set; } = [];
    [Array(CountBits = 32)] public List<Server0112Part3> Unknown6 { get; set; } = [];
}
