using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

/// <summary>
/// The <see cref="AuctionFilterType.PropertyMin"/> filter: an item property and its minimum.
/// </summary>
[WireStruct]
[UnionCase(0)]
public sealed partial class Client07DCUnion0Arm00 : IClient07DCUnion0
{
    /// <summary>
    /// The item property.
    /// </summary>
    [UInt(8)] public byte Property { get; private set; }

    /// <summary>
    /// The property's minimum.
    /// </summary>
    [Float32] public float Min { get; private set; }
}
