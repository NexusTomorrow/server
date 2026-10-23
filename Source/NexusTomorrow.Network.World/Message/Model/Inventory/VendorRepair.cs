using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// Repairs items at a vendor: the request RepairItemVendor and RepairAllItemsVendor send, and their GuildTypeLib counterparts
/// when the guild pays.
/// </summary>
/// <remarks>
/// The client sends it only while the player is at a vendor.
/// </remarks>
[WireMessage(GameMessageOpcode.Client014C, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class VendorRepair
{
    /// <summary>
    /// The item to repair, by id; 0 repairs everything.
    /// </summary>
    [UInt(64)] public ulong ItemId { get; set; }

    /// <summary>
    /// The guild paying for the repair, by its id within the realm; 0 when the player pays.
    /// </summary>
    [UInt(64)] public ulong GuildId { get; set; }

    [UInt(64)] public ulong Unknown2 { get; set; }
}
