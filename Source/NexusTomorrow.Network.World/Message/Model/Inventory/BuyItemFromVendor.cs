using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Buys an item from the vendor: the request the global script call BuyItemFromVendor sends.
/// </summary>
/// <remarks>
/// The client sends it only while the player interacts with a vendor.
/// </remarks>
[WireMessage(GameMessageOpcode.Client00BE, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class BuyItemFromVendor
{
    /// <summary>
    /// The item to buy, by its unique id: the script call's idUnique.
    /// </summary>
    [UInt(32)] public uint ItemId { get; set; }

    /// <summary>
    /// How many to buy: the script call's nQuantity.
    /// </summary>
    [UInt(32)] public uint Quantity { get; set; }
}
