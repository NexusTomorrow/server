using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

/// <summary>
/// One filter of an <see cref="ItemAuctionSearch"/>: its type, and its values in the arm the type picks.
/// </summary>
[WireStruct]
public sealed partial class Client07DCPart0 : IClient07DCUnion0
{
    /// <summary>
    /// The filter's type, which picks its arm.
    /// </summary>
    [UInt(3)] public AuctionFilterType Type { get; private set; }

    /// <summary>
    /// The filter's values, by its type.
    /// </summary>
    [Union(nameof(Type))] public IClient07DCUnion0 Payload { get; private set; } = new Client07DCUnion0Arm00();
}
