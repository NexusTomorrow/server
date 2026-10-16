using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0405Part0
{
    public Client00B5Part0 Unknown0 { get; set; } = new();
    [UInt(32)] public uint Unknown1 { get; set; }
    public Server0405Part1 Unknown2 { get; set; } = new();
    [UInt(32)] public uint Unknown3 { get; set; }
}
