using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

/// <summary>
/// The <see cref="AuctionFilterType.PropertyMax"/> filter: an item property and a value.
/// </summary>
[WireStruct]
[UnionCase(1)]
public sealed partial class Client07DCUnion0Arm01 : IClient07DCUnion0
{
    /// <summary>
    /// The item property.
    /// </summary>
    [UInt(8)] public byte Property { get; private set; }

    [Float32] public float Unknown1 { get; private set; }
}
