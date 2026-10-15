using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[Message(GameMessageOpcode.ServerMailUnavailable)]
[WireMessage(GameMessageOpcode.ServerMailUnavailable, Direction.ServerToClient)]
public sealed partial class ServerMailUnavailable : IWritable
{
    [UInt(64)] public ulong MailId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(MailId);
    }
}
