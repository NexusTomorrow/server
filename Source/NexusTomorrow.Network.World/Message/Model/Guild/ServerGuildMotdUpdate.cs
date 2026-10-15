using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild
{
    [Message(GameMessageOpcode.ServerGuildMotdUpdate)]
    public class ServerGuildMotdUpdate : IWritable
    {
        public Identity GuildIdentity { get; set; }
        public string MessageOfTheDay { get; set; }

        public void Write(GamePacketWriter writer)
        {
            GuildIdentity.Write(writer);
            writer.WriteStringWide(MessageOfTheDay);
        }
    }
}
