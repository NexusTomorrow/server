using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerBuybackItemUpdated, Direction.ServerToClient)]
public sealed partial class ServerBuybackItemUpdated
{
    public BuybackItem BuybackItem { get; set; }
}
