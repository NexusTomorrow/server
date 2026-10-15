using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Mail
{
    [Message(GameMessageOpcode.ServerMailTakeAttachment)]
    public class ServerMailTakeAttachment : IWritable
    {
        public ulong MailId { get; set; }
        public GenericError Result { get; set; }
        public uint Index { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(MailId);
            writer.Write(Result, 8u);
            writer.Write(Index);
        }
    }
}
