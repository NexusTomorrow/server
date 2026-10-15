using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerBuybackItemUpdated)]
[WireMessage(GameMessageOpcode.ServerBuybackItemUpdated, Direction.ServerToClient)]
public sealed partial class ServerBuybackItemUpdated : IWritable
{
    public BuybackItem BuybackItem { get; set; }

    public void Write(GamePacketWriter writer)
    {
        BuybackItem.Write(writer);
    }
}
