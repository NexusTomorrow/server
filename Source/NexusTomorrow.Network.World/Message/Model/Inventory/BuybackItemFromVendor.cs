using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Buys back an item sold to the vendor: the request the global script call BuybackItemFromVendor sends.
/// </summary>
/// <remarks>
/// The client sends it only while the player interacts with a vendor.
/// </remarks>
[WireMessage(GameMessageOpcode.Client00BB, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class BuybackItemFromVendor
{
    /// <summary>
    /// The script call's idItem, documented as the unique id of the item being bought back.
    /// </summary>
    [UInt(32)] public uint BuybackItemId { get; private set; }
}
