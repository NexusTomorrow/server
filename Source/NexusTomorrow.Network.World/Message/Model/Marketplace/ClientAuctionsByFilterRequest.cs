using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.ClientAuctionsByFilterRequest, Direction.ClientToServer)]
public sealed partial class ClientAuctionsByFilterRequest
{
    // Only one of the four fields below will be filled out depedning on the search type
    // Family, Category, Type, or by item(s)
    [UInt(14)] public uint Item2FamilyId { get; private set; }
    [UInt(14)] public uint Item2CategoryId { get; private set; }
    [UInt(14)] public uint Item2TypeId { get; private set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Item2Ids { get; private set; } = [];

    // Each filter keeps its 3-bit type, which selects the arm in Filter.
    [Array(CountBits = 32)] public List<AuctionFilter> Filters { get; private set; } = [];
    [UInt(32)] public uint ReverseSort { get; private set; } // is a flag array but only lowest bit is used
    [UInt(3)] public AuctionSort AuctionSort { get; private set; }
    [UInt(8)] public Property SortPropertyId { get; private set; } // required if AuctionSort is Property
    [UInt(32)] public uint Page { get; private set; }
}
