using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Server00BA, Direction.ServerToClient)]
public sealed partial class BuybackItemUpdated
{
    public Server00BAPart0 Unknown0 { get; set; } = new();
}
