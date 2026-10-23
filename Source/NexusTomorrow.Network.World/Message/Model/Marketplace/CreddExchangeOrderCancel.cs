using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Cancels one of the player's CREDD exchange orders.
/// </summary>
[WireMessage(GameMessageOpcode.Client0267, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class CreddExchangeOrderCancel
{
    /// <summary>
    /// The client always sends 0.
    /// </summary>
    public RealmScopedId Unknown0 { get; private set; } = new();

    /// <summary>
    /// The order, by <see cref="CreddExchangeInfo.CreddExchangeOrder.OrderId"/>.
    /// </summary>
    [UInt(64)] public ulong OrderId { get; private set; }
}
