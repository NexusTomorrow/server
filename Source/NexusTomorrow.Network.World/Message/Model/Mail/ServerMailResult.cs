using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Mail
{
    [Message(GameMessageOpcode.ServerMailResult)]
    public class ServerMailResult : IWritable
    {
        // actions 2 and 3 invoke RefreshMail LUA event
        public uint Action { get; set; } // TODO: Analyze sniffs for possible values
        public ulong MailId { get; set; }
        public GenericError Result { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Action);
            writer.Write(MailId);
            writer.Write(Result, 8u);
        }
    }
}
