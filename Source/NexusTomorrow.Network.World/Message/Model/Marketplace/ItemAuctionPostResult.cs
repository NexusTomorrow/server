using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server06DD, Direction.ServerToClient)]
public sealed partial class ItemAuctionPostResult
{
    [UInt(8)] public byte Unknown0 { get; set; }
    public ItemAuction Unknown1 { get; set; } = new();
}
