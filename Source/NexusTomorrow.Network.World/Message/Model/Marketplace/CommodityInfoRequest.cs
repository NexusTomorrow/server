using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Asks for one item's commodity market, which <see cref="CommodityInfoResults"/> answers.
/// </summary>
/// <remarks>
/// MarketplaceLib.RequestCommodityInfo sends it, and sends nothing for item 0.
/// </remarks>
[WireMessage(GameMessageOpcode.Client03E6, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CommodityInfoRequest
{
    /// <summary>
    /// The item, an Item2 table id; <see cref="CommodityInfoResults.ItemId"/> echoes it.
    /// </summary>
    [UInt(18)] public uint ItemId { get; private set; }
}
