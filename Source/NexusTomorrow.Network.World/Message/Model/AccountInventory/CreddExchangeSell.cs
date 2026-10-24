using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Submits an order to sell CREDD on the CREDD exchange; the client raises CREDDExchangeOrderSubmitted as it sends it.
/// </summary>
[WireMessage(GameMessageOpcode.Client026B, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CreddExchangeSell
{
    /// <summary>
    /// The client gives this value no meaning. It always sends 0.
    /// </summary>
    public RealmScopedId Unknown0 { get; private set; } = new();

    /// <summary>
    /// The price, in credits.
    /// </summary>
    [UInt(64)] public ulong Price { get; private set; }

    /// <summary>
    /// Set when the player chose the exchange's sell-now; clear when the player posts a sell order.
    /// </summary>
    [UInt(1)] public bool IsInstant { get; private set; }
}
