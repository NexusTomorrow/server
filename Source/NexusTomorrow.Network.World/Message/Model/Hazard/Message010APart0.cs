using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

[WireStruct]
public sealed partial class Message010APart0
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [UInt(3)] public byte Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(18)] public uint Unknown5 { get; set; }
    [UInt(32)] public uint Unknown6 { get; set; }
    [UInt(32)] public uint Unknown7 { get; set; }
    [UInt(1)] public bool Unknown8 { get; set; }
    [UInt(1)] public bool Unknown9 { get; set; }
    [UInt(1)] public bool Unknown10 { get; set; }
    [UInt(1)] public bool Unknown11 { get; set; }
    [UInt(1)] public bool Unknown12 { get; set; }
}
