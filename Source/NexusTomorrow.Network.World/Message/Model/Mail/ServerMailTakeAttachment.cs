using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Mail;

[Message(GameMessageOpcode.ServerMailTakeAttachment)]
[WireMessage(GameMessageOpcode.ServerMailTakeAttachment, Direction.ServerToClient)]
public sealed partial class ServerMailTakeAttachment : IWritable
{
    [UInt(64)] public ulong MailId { get; set; }
    [UInt(8)] public GenericError Result { get; set; }
    [UInt(32)] public uint Index { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(MailId);
        writer.Write(Result, 8u);
        writer.Write(Index);
    }
}
