using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Chat
{

    [Message(GameMessageOpcode.ServerChatZoneChange)]
    public class ServerChatZoneChange : IWritable
    {
       public ushort WorldZoneId { get; set; }

       public void Write(GamePacketWriter writer)
       {
           writer.Write(WorldZoneId, 15u);
       }
    }
}
