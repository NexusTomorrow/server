using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Asks for the player's own commodity orders, which <see cref="OwnedCommodityOrders"/> answers.
/// </summary>
[WireMessage(GameMessageOpcode.Client03EC, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class OwnedCommodityOrdersRequest
{
}
