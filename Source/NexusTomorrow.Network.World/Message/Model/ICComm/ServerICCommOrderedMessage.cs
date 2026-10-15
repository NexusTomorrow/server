using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

// Not seen in sniffs so not sure when these would be sent
[Message(GameMessageOpcode.ServerICCommOrderedMessage)]
public class ServerICCommOrderedMessage : IWritable
{
    public ulong IccomId { get; set; }
    public uint MessageId { get; set; }
    public string Message { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(IccomId);
        writer.Write(MessageId);
        writer.WriteStringWide(Message);
    }
}
