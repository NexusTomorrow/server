using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerItemSwap, Direction.ServerToClient)]
public sealed partial class ServerItemSwap
{
    public ItemDragDrop To { get; set; }
    public ItemDragDrop From { get; set; }
}
