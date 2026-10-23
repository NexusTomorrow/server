using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// The player's own item auctions; the client raises OwnedItemAuctions with them.
/// </summary>
[WireMessage(GameMessageOpcode.Server064D, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class OwnedItemAuctions
{
    /// <summary>
    /// The auctions.
    /// </summary>
    [Array(CountBits = 32)] public List<ItemAuction> Auctions { get; set; } = [];
}
