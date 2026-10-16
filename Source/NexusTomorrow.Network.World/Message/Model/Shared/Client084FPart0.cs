using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Client084FPart0
{
    [FixedArray(5), UInt(8)] public byte[] Unknown0 { get; set; } = new byte[5];
    [UInt(8)] public byte Unknown1 { get; set; }
    [UInt(8)] public byte Unknown2 { get; set; }
    [UInt(8)] public byte Unknown3 { get; set; }
}
