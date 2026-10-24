using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// An item waiting to be claimed, alone or in a named group.
/// </summary>
[WireStruct]
public sealed partial class AccountPendingItem
{
    /// <summary>
    /// The pending item, by its own id; <see cref="AccountInventory.AccountPendingItemRemove"/> removes an ungrouped item by it. The
    /// client drops an item whose id it already holds.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }

    /// <summary>
    /// What the item is, an AccountItem table id.
    /// </summary>
    [UInt(32)] public uint AccountItemId { get; set; }

    [UInt(64)] public ulong Unknown2 { get; set; }

    /// <summary>
    /// The group the item joins, by name, or empty for an ungrouped item. The claim, return and gift requests and
    /// <see cref="AccountInventory.AccountPendingItemGroupRemove"/> name the group by it.
    /// </summary>
    [WideString] public string GroupName { get; set; } = "";

    [UInt(32)] public uint Unknown4 { get; set; }
    public RealmScopedId Unknown5 { get; set; } = new();
    [UInt(64)] public ulong Unknown6 { get; set; }
    [UInt(5)] public byte Unknown7 { get; set; }

    /// <summary>
    /// The item's gift or return time: a Windows FILETIME, 100-nanosecond intervals since 1601-01-01 UTC. A group's time runs to the
    /// earliest among its returnable items, which add-ons see as giftReturnTimeRemaining.
    /// </summary>
    [UInt(64)] public ulong GiftReturnTime { get; set; }

    /// <summary>
    /// The client gives this value no meaning. It discards it.
    /// </summary>
    public RealmScopedId Unknown9 { get; set; } = new();
}
