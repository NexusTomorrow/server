using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[Message(GameMessageOpcode.ClientMailOpen)]
[WireMessage(GameMessageOpcode.ClientMailOpen, Direction.ClientToServer)]
public sealed partial class ClientMailOpen : IReadable
{
    [Array(CountBits = 32), UInt(64)] public List<ulong> MailList { get; } = [];

    public void Read(GamePacketReader reader)
    {
        uint count = reader.ReadUInt();
        for (int i = 0; i < count; i++)
            MailList.Add(reader.ReadULong());
    }
}
