using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Client0182, Direction.ClientToServer)]
public sealed partial class ItemMove
{
    public ItemLocation Unknown0 { get; } = new();
    public ItemLocation Unknown1 { get; } = new();
}
