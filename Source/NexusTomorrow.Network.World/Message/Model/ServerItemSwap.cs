using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerItemSwap)]
[WireMessage(GameMessageOpcode.ServerItemSwap, Direction.ServerToClient)]
public sealed partial class ServerItemSwap : IWritable
{
    public ItemDragDrop To { get; set; }
    public ItemDragDrop From { get; set; }

    public void Write(GamePacketWriter writer)
    {
        To.Write(writer);
        From.Write(writer);
    }
}
