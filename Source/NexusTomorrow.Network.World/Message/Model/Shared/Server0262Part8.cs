using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0262Part8
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(8)] public byte Unknown1 { get; set; }
    [UInt(8)] public byte Unknown2 { get; set; }
    [UInt(16)] public ushort Unknown3 { get; set; }
    [UInt(4)] public byte Unknown4 { get; set; }

    [Array(CountBits = 8)] public List<Server0262Part9> Unknown5 { get; set; } = [];

}
