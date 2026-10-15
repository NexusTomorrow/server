using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[Message(GameMessageOpcode.ServerMailItemDeprecation)]
public class ServerMailItemDeprecation : IWritable
{
    public List<ulong> MailIds { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(MailIds.Count);
        MailIds.ForEach(mailId => writer.Write(mailId));
    }
}
