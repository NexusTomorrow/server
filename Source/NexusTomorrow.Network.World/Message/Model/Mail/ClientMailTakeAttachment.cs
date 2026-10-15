using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[Message(GameMessageOpcode.ClientMailTakeAttachment)]
[WireMessage(GameMessageOpcode.ClientMailTakeAttachment, Direction.ClientToServer)]
public sealed partial class ClientMailTakeAttachment : IReadable
{
    [UInt(64)] public ulong MailId { get; private set; }
    [UInt(32)] public uint Index { get; private set; }
    [UInt(32)] public uint MailboxUnitId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        MailId = reader.ReadULong();
        Index = reader.ReadUInt();
        MailboxUnitId = reader.ReadUInt();
    }
}
