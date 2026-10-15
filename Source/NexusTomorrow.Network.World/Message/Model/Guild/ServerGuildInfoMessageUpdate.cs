using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild
{
    [Message(GameMessageOpcode.ServerGuildInfoMessageUpdate)]
    public class ServerGuildInfoMessageUpdate : IWritable
    {
        public Identity GuildIdentity { get; set; }
        public string InfoMessage { get; set; }

        public void Write(GamePacketWriter writer)
        {
            GuildIdentity.Write(writer);
            writer.WriteStringWide(InfoMessage);
        }
    }
}
