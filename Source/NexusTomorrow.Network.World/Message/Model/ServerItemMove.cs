using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerItemMove, Direction.ServerToClient)]
public sealed partial class ServerItemMove
{
    public ItemDragDrop To { get; set; }
}
