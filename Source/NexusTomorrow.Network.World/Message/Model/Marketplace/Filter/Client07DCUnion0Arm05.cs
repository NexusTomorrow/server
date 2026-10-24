using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

/// <summary>
/// The <see cref="AuctionFilterType.BuyoutMax"/> filter: the highest buyout price.
/// </summary>
[WireStruct]
[UnionCase(5)]
public sealed partial class Client07DCUnion0Arm05 : IClient07DCUnion0
{
    /// <summary>
    /// The highest buyout price, above zero.
    /// </summary>
    [UInt(64)] public ulong Max { get; private set; }
}
