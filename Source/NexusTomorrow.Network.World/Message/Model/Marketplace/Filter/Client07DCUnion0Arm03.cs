using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

/// <summary>
/// The <see cref="AuctionFilterType.Quality"/> filter: a range of item qualities, each 0 to 7.
/// </summary>
[WireStruct]
[UnionCase(3)]
public sealed partial class Client07DCUnion0Arm03 : IClient07DCUnion0
{
    /// <summary>
    /// The lowest quality.
    /// </summary>
    [UInt(4)] public byte Min { get; private set; }

    /// <summary>
    /// The highest quality.
    /// </summary>
    [UInt(4)] public byte Max { get; private set; }
}
