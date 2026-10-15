using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Message.Model.Shared;

[WireStruct]
public sealed partial class Message
{
    [UInt(32)] public uint Index { get; set; }
    [Array(CountBits = 8), WideString] public List<string> Messages { get; set; } = [];
}
