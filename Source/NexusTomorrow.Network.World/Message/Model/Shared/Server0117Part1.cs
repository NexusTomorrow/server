using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0117Part1
{
    [UInt(7)] public byte Unknown0 { get; set; }
    [UInt(15)] public ushort Unknown1 { get; set; }
    [UInt(14)] public ushort Unknown2 { get; set; }
    [UInt(32)] public uint Unknown3 { get; set; }
}
