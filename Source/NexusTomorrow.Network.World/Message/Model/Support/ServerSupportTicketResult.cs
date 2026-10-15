using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support
{
    [Message(GameMessageOpcode.ServerSupportTicketResult)]
    public class ServerSupportTicketResult : IWritable
    {
        public bool Success { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Success);
        }
    }
}
