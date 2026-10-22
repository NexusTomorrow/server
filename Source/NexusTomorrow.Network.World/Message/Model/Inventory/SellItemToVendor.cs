using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Sells an item to the vendor: the request the global script calls SellItemToVendor and SellItemToVendorById send, among
/// others.
/// </summary>
/// <remarks>
/// The client sends it only while the player interacts with a vendor.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0166, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class SellItemToVendor
{
    /// <summary>
    /// Where the item is.
    /// </summary>
    public ItemLocation Location { get; } = new();

    /// <summary>
    /// How many to sell.
    /// </summary>
    [UInt(32)] public uint Quantity { get; private set; }
}
