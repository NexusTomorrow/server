using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireStruct]
public sealed partial class Server07C7Part0
{
    [UInt(3)] public byte Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(8)] public byte Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
}
