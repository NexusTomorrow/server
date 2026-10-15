using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[Message(GameMessageOpcode.ClientMailReturn)]
public class ClientMailReturn : IReadable
{
    public ulong MailId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        MailId = reader.ReadULong();
    }
}
