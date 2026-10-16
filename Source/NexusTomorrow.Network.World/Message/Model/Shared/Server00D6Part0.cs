using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server00D6Part0
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 5), UInt(32)] public List<uint> Unknown1 { get; set; } = [];
}
