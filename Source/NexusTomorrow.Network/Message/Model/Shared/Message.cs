using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model.Shared;

[WireStruct]
public sealed partial class Message0761Part2
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 8), WideString] public List<string> Unknown1 { get; set; } = [];
}
