using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Searches the auction house by item family, category, type or a list of items, with filters, a sort and a page.
/// </summary>
/// <remarks>
/// MarketplaceLib.RequestItemAuctionsByFamily, ByCategory, ByType and ByItems each set one of <see cref="FamilyId"/>,
/// <see cref="CategoryId"/>, <see cref="TypeId"/> and <see cref="ItemIds"/> and leave the others empty; the client sends nothing when all
/// four are empty.
/// </remarks>
[WireMessage(GameMessageOpcode.Client07DC, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class ItemAuctionSearch
{
    /// <summary>
    /// The item family searched, or 0.
    /// </summary>
    [UInt(14)] public uint FamilyId { get; private set; }

    /// <summary>
    /// The item category searched, or 0.
    /// </summary>
    [UInt(14)] public uint CategoryId { get; private set; }

    /// <summary>
    /// The item type searched, or 0.
    /// </summary>
    [UInt(14)] public uint TypeId { get; private set; }

    /// <summary>
    /// The items searched, up to 10 Item2 table ids, or none.
    /// </summary>
    [Array(CountBits = 32), UInt(32)] public List<uint> ItemIds { get; private set; } = [];

    /// <summary>
    /// The filters, up to six.
    /// </summary>
    [Array(CountBits = 32)] public List<Client07DCPart0> Filters { get; private set; } = [];

    /// <summary>
    /// Flags. Bit 0 reverses the sort. Bits 1 and 2 also come from the request, and no add-on sets them.
    /// </summary>
    [UInt(32)] public uint Flags { get; private set; }

    /// <summary>
    /// What the results are sorted by.
    /// </summary>
    [UInt(3)] public AuctionSort Sort { get; private set; }

    /// <summary>
    /// The item property to sort by when <see cref="Sort"/> is <see cref="AuctionSort.Property"/>.
    /// </summary>
    [UInt(8)] public byte SortProperty { get; private set; }

    /// <summary>
    /// The page of results asked for.
    /// </summary>
    [UInt(32)] public uint Page { get; private set; }
}
