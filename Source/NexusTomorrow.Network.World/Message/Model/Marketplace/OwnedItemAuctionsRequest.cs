using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Asks for the player's own item auctions, which <see cref="OwnedItemAuctions"/> answers.
/// </summary>
[WireMessage(GameMessageOpcode.Client03ED, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class OwnedItemAuctionsRequest
{
}
