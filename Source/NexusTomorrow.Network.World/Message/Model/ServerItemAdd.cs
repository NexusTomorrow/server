using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerItemAdd, Direction.ServerToClient)]
public sealed partial class ServerItemAdd
{
    public InventoryItem InventoryItem { get; set; }
}
