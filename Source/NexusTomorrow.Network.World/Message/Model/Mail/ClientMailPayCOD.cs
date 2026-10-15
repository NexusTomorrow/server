using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[Message(GameMessageOpcode.ClientMailPayCod)]
[WireMessage(GameMessageOpcode.ClientMailPayCod, Direction.ClientToServer)]
public sealed partial class ClientMailPayCod : IReadable
{
    [UInt(64)] public ulong MailId { get; private set; }
    [UInt(32)] public uint MailboxUnitId { get; private set; } // Must be interacting with a mailbox to pay COD

    public void Read(GamePacketReader reader)
    {
        MailId = reader.ReadULong();
        MailboxUnitId = reader.ReadUInt();
    }
}
