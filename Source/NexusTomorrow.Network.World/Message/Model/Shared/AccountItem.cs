using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// An item in the account's inventory, or one in progress.
/// </summary>
[WireStruct]
public sealed partial class AccountItem
{
    /// <summary>
    /// The item, by its own id: the key <see cref="AccountInventory.AccountItemTake"/> sends back, and the one
    /// <see cref="AccountInventory.AccountItemRemove"/> and <see cref="AccountInventory.AccountItemInProgressRemove"/> remove by. The
    /// client drops an item whose id it already holds.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }

    /// <summary>
    /// What the item is, an AccountItem table id.
    /// </summary>
    [UInt(32)] public uint AccountItemId { get; set; }

    /// <summary>
    /// The client gives this value no meaning. It discards it.
    /// </summary>
    [UInt(5)] public byte Unknown2 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. It discards it.
    /// </summary>
    [UInt(1)] public bool Unknown3 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. It discards it.
    /// </summary>
    public RealmScopedId Unknown4 { get; set; } = new();
}
