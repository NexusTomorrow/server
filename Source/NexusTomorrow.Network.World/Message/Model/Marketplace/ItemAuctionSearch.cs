using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Marketplace.Filter;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Client07DC, Direction.ClientToServer)]
public sealed partial class ItemAuctionSearch
{
    [UInt(14)] public uint Unknown0 { get; private set; }
    [UInt(14)] public uint Unknown1 { get; private set; }
    [UInt(14)] public uint Unknown2 { get; private set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown3 { get; private set; } = [];

    [Array(CountBits = 32)] public List<Client07DCPart0> Unknown4 { get; private set; } = [];
    [UInt(32)] public uint Unknown5 { get; private set; }
    [UInt(3)] public byte Unknown6 { get; private set; }
    [UInt(8)] public byte Unknown7 { get; private set; }
    [UInt(32)] public uint Unknown8 { get; private set; }
}
