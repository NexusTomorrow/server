using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

[WireMessage(GameMessageOpcode.Server0568, Direction.ServerToClient)]
public sealed partial class ItemsSwapped
{
    public ItemPlacement Unknown0 { get; set; } = new();
    public ItemPlacement Unknown1 { get; set; } = new();
}
