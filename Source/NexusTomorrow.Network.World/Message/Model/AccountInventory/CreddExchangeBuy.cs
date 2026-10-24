using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Submits an order to buy CREDD on the CREDD exchange; the client raises CREDDExchangeOrderSubmitted as it sends it.
/// </summary>
[WireMessage(GameMessageOpcode.Client0265, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CreddExchangeBuy
{
    /// <summary>
    /// The price, in credits.
    /// </summary>
    [UInt(64)] public ulong Price { get; private set; }

    /// <summary>
    /// Set when the player chose the exchange's buy-now; clear when the player posts a buy order.
    /// </summary>
    [UInt(1)] public bool IsInstant { get; private set; }
}
