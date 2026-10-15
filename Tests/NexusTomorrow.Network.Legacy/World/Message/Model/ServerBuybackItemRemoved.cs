using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerBuybackItemRemoved)]
public class ServerBuybackItemRemoved : IWritable
{
    public uint UniqueId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UniqueId);
    }
}
