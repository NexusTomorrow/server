using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

/// <summary>
/// The <see cref="AuctionFilterType.EquippableBy"/> filter.
/// </summary>
[WireStruct]
[UnionCase(6)]
public sealed partial class Client07DCUnion0Arm06 : IClient07DCUnion0
{
    [UInt(14)] public ushort Unknown0 { get; private set; }
    [UInt(14)] public ushort Unknown1 { get; private set; }
    [UInt(14)] public ushort Unknown2 { get; private set; }
    [UInt(32)] public uint Unknown3 { get; private set; }
}
