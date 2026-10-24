using NexusTomorrow.Game.Static.Item;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// One item. The client files it in the player's inventory (<see cref="ItemUpdate"/>), in an inspected unit's equipment
/// (<see cref="Entity.InspectResponse"/>) and in a guild bank tab (<see cref="Guild.GuildBankItemAdded"/>,
/// <see cref="Guild.GuildBankTabItems"/>).
/// </summary>
[WireStruct]
public sealed partial class ItemInstance
{
    /// <summary>
    /// A price a vendor pays back for the item while its return window is open.
    /// </summary>
    [WireStruct]
    public sealed partial class ReturnPrice
    {
        [UInt(3)] public byte Unknown0 { get; set; }

        /// <summary>
        /// The price's amount.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint Amount { get; set; }

        [UInt(32)] public uint Unknown2 { get; set; }
    }

    /// <summary>
    /// The item's unique id. Later messages name the item by it (<see cref="Inventory.ItemRemoved"/>,
    /// <see cref="Inventory.ItemStackCountChanged"/>, <see cref="Entity.ItemMoved"/>, <see cref="Entity.ItemsSwapped"/> and others), and
    /// the guild bank keys its items by it.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong ItemId { get; set; }

    /// <summary>
    /// The character who crafted the item, by character id on the player's realm, or 0 for none. The client looks the character's name up
    /// and shows it on the item's tooltip as its maker.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong MakerCharacterId { get; set; }

    /// <summary>
    /// The item, as an id in the client's Item2 table; the client refuses an item whose row it lacks. At location 4, the ability list, the
    /// client takes it as a spell's id instead.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint Item2Id { get; set; }

    /// <summary>
    /// Where the item sits. The client refuses location 300 or above, a slot of 0xFFFFFFFF, and a slot of 31 or above in location 0.
    /// Location 4 is the ability list, which the client keeps apart from the inventory.
    /// </summary>
    [VerifiedMember] public ItemLocation Location { get; set; } = new();

    /// <summary>
    /// How many are in the stack. Item:GetStackCount reports 0 as 1.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint StackCount { get; set; }

    /// <summary>
    /// The item's charges, which Item:GetCharges reports.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Charges { get; set; }

    /// <summary>
    /// With bits 56 to 63 at 1, the client shows the item's own rune slot elements, <see cref="RuneSlotElements"/>; at 0, it shows the item
    /// row's socket types.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong Unknown6 { get; set; }

    [UInt(3)] public byte Unknown7Bits0To2 { get; set; }
    [UInt(3)] public byte Unknown7Bits3To5 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. It stores the bit and never reads it.
    /// </summary>
    [UInt(1)] public bool Unknown7Bit6 { get; set; }

    /// <summary>
    /// Each entry is the element of that rune slot, slot 1 first. The client uses it only while the top byte of <see cref="Unknown6"/>
    /// (bits 56 to 63) is above 0; otherwise it shows the item row's own socket types.
    /// </summary>
    [VerifiedMember, FixedArray(8), UInt(3)] public byte[] RuneSlotElements { get; set; } = new byte[8];

    /// <summary>
    /// The client gives this value no meaning. Its decoders stop at bit 30 and never extract it.
    /// </summary>
    [UInt(1)] public bool Unknown7Bit31 { get; set; }

    /// <summary>
    /// How many of the item's imbuements are complete. The client applies the rewards of that many and marks them complete on the item's
    /// tooltip.
    /// </summary>
    [VerifiedMember, UInt(4)] public byte CompletedImbuements { get; set; }

    [UInt(10)] public ushort Unknown8Bits4To13 { get; set; }
    [UInt(10)] public ushort Unknown8Bits14To23 { get; set; }
    [UInt(10)] public ushort Unknown8Bits24To33 { get; set; }

    /// <summary>
    /// The client gives this value no meaning. Its decoders never extract bits 34 to 63.
    /// </summary>
    [UInt(30)] public uint Unknown8Bits34To63 { get; set; }

    /// <summary>
    /// The durability left, as a fraction of the item row's maximum; 1.0 is full.
    /// </summary>
    [VerifiedMember, Float32] public float Durability { get; set; }

    [UInt(32)] public uint Unknown10 { get; set; }

    /// <summary>
    /// The item's flags. The client never tests 0x02 or 0x04.
    /// </summary>
    [VerifiedMember, UInt(8)] public ItemInstanceFlags Flags { get; set; }

    /// <summary>
    /// Milliseconds left in the item's vendor return window, 0 for none. The client keeps <see cref="ReturnPrices"/> only while it is
    /// not 0.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint ReturnTimeRemainingMs { get; set; }

    /// <summary>
    /// Milliseconds until a timed item expires, 0 for none.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint DurationRemainingMs { get; set; }

    /// <summary>
    /// Milliseconds left in which a soulbound item may still go to the characters <see cref="SoulboundTradeAllowedTo"/> lists.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint SoulboundTradeTimeRemainingMs { get; set; }

    /// <summary>
    /// The prices a vendor pays back while the item's return window is open. Item:GetSellPrice gives these instead of the item row's price
    /// while <see cref="ItemInstanceFlags.NotReturnable"/> is clear.
    /// </summary>
    [VerifiedMember, FixedArray(2)] public ReturnPrice[] ReturnPrices { get; set; } = [new(), new()];

    /// <summary>
    /// The power core applied to the item, as an id in the client's Item2 table. When that item is a power core, the client applies its
    /// level range to the stats it works out for this item.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint PowerCoreItem2Id { get; set; }

    [Array(CountBits = 3), UInt(32)] public List<uint> Unknown17 { get; } = [];

    /// <summary>
    /// One rune item id per slot, in slot order. The client applies each one its slot accepts, up to the item's rune slot count.
    /// </summary>
    [VerifiedMember, Array(CountBits = 4), UInt(32)] public List<uint> RuneItemIds { get; } = [];

    /// <summary>
    /// The characters a soulbound item may still be traded to while <see cref="SoulboundTradeTimeRemainingMs"/> runs. Item:IsTradeableTo
    /// matches the trade partner against each.
    /// </summary>
    [VerifiedMember, Array(CountBits = 6)] public List<RealmScopedId> SoulboundTradeAllowedTo { get; } = [];

    /// <summary>
    /// For an item row that scales, the item's level, which its tooltip shows; 0 keeps the row's.
    /// </summary>
    [VerifiedMember, UInt(16)] public ushort ItemLevel { get; set; }

    /// <summary>
    /// For an item row that scales, the item's quality, which its tooltip shows; 0 keeps the row's.
    /// </summary>
    [VerifiedMember, UInt(8)] public byte Quality { get; set; }

    /// <summary>
    /// For an item row that scales, the item's prime tier, which its tooltip shows; 0 keeps the row's.
    /// </summary>
    [VerifiedMember, UInt(8)] public byte PrimeTier { get; set; }
}
