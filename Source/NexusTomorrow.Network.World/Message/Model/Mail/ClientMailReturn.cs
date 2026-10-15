using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[Message(GameMessageOpcode.ClientMailReturn)]
[WireMessage(GameMessageOpcode.ClientMailReturn, Direction.ClientToServer)]
public sealed partial class ClientMailReturn : IReadable
{
    [UInt(64)] public ulong MailId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        MailId = reader.ReadULong();
    }
}
