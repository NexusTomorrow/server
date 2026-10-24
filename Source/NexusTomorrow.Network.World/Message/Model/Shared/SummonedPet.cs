using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class SummonedPet
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(18)] public uint Unknown1 { get; set; }
    [UInt(5)] public byte Unknown2 { get; set; }
    [UInt(5)] public byte Unknown3 { get; set; }
}
