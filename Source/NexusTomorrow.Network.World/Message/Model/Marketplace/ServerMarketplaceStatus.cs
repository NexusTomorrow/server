using NexusTomorrow.Game.Static.Marketplace;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[Message(GameMessageOpcode.ServerMarketplaceStatus)]
[WireMessage(GameMessageOpcode.ServerMarketplaceStatus, Direction.ServerToClient)]
public sealed partial class ServerMarketplaceStatus : IWritable
{
    [UInt(32)] public MarketplaceStatus Status { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Status, 32u);
    }
}
