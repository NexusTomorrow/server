using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Client0149, Direction.ClientToServer)]
public sealed partial class ItemDelete
{
    public ItemLocation Unknown0 { get; } = new();
}
