using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[Message(GameMessageOpcode.ClientMailTakeAllFromSelection)]
[WireMessage(GameMessageOpcode.ClientMailTakeAllFromSelection, Direction.ClientToServer)]
public sealed partial class ClientMailTakeAllFromSelection : IReadable
{
    [Array(CountBits = 32), UInt(64)] public List<ulong> MailList { get; } = [];
    [UInt(32)] public uint MailboxUnitId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        uint count = reader.ReadUInt();
        for (int i = 0; i < count; i++)
        {
            MailList.Add(reader.ReadULong());
        }

        MailboxUnitId = reader.ReadUInt();
    }
}
