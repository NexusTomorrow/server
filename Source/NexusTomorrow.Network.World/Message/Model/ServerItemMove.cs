using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerItemMove)]
[WireMessage(GameMessageOpcode.ServerItemMove, Direction.ServerToClient)]
public sealed partial class ServerItemMove : IWritable
{
    public ItemDragDrop To { get; set; }

    public void Write(GamePacketWriter writer)
    {
        To.Write(writer);
    }
}
