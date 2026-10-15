using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[Message(GameMessageOpcode.ServerMailItemDeprecation)]
[WireMessage(GameMessageOpcode.ServerMailItemDeprecation, Direction.ServerToClient)]
public sealed partial class ServerMailItemDeprecation : IWritable
{
    [Array(CountBits = 32), UInt(64)] public List<ulong> MailIds { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(MailIds.Count);
        MailIds.ForEach(mailId => writer.Write(mailId));
    }
}
