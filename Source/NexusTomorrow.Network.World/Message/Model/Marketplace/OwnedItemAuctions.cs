using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server064D, Direction.ServerToClient)]
public sealed partial class OwnedItemAuctions
{
    [Array(CountBits = 32)] public List<ItemAuction> Unknown0 { get; set; } = [];
}
