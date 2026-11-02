using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

/// <summary>
/// Sets an inventory item's charges by item id.
/// </summary>
[WireMessage(GameMessageOpcode.Server00CA, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ItemChargesUpdated
{
    /// <summary>
    /// The item, by id; the client finds it in the inventory and ignores a missing one.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong ItemId { get; set; }

    /// <summary>
    /// The item's charges.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Charges { get; set; }
}
