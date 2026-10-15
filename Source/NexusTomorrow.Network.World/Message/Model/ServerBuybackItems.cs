using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerBuybackItems, Direction.ServerToClient)]
public sealed partial class ServerBuybackItems
{
    [Array(CountBits = 32)] public List<BuybackItem> BuybackItems { get; } = [];
}
