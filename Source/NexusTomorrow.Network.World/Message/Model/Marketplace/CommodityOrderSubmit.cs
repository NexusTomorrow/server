using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Posts a commodity order, to buy or to sell by <see cref="CommodityOrder.IsBuy"/>.
/// </summary>
/// <remarks>
/// The client sends it only for an order it can post, with the whole order as the add-on built it.
/// </remarks>
[WireMessage(GameMessageOpcode.Client06DA, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class CommodityOrderSubmit
{
    /// <summary>
    /// The order.
    /// </summary>
    public CommodityOrder Order { get; private set; } = new();
}
