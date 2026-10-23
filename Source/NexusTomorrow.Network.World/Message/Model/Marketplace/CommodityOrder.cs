using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireStruct]
public sealed partial class CommodityOrder
{
    [UInt(64)] public ulong Unknown0 { get; set; }
    [UInt(18)] public uint Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
    [UInt(64)] public ulong Unknown3 { get; set; }
    [UInt(64)] public ulong Unknown4 { get; set; }
    [UInt(1)] public bool Unknown5 { get; set; }
    [UInt(1)] public bool Unknown6 { get; set; }
    [UInt(64)] public ulong Unknown7 { get; set; }
    [UInt(64)] public ulong Unknown8 { get; set; }
}
