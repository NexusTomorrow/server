using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.ServerMarketplaceStatus, Direction.ServerToClient)]
public sealed partial class ServerMarketplaceStatus
{
    [UInt(32)] public MarketplaceStatus Status { get; set; }
}
