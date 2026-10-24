using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// An item with the reason it was added, as <see cref="Inventory.ItemAdded"/> and <see cref="WorldEntry.PlayerState"/> carry it.
/// </summary>
[WireStruct]
public sealed partial class ItemUpdate
{
    /// <summary>
    /// The item.
    /// </summary>
    [VerifiedMember] public ItemInstance Item { get; set; } = new();

    /// <summary>
    /// Why the item was added: an Item.CodeEnumItemUpdateReason value. The client itself uses 0x31, which that enum does not name, when it
    /// clears an occupied location, and then raises no AbilityBookChange.
    /// </summary>
    [VerifiedMember, UInt(6)] public byte Reason { get; set; }
}
