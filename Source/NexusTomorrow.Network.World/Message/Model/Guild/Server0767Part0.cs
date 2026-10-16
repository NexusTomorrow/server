using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireStruct]
public sealed partial class Server0767Part0
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [WideString] public string Unknown1 { get; set; } = "";
    [WideString] public string Unknown2 { get; set; } = "";
    public Server0767Part1 Unknown3 { get; set; } = new();
    [Array(CountBits = 32), WideString] public List<string> Unknown4 { get; set; } = [];
}
