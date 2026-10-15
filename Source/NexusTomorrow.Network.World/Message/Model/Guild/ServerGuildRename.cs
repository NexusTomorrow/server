using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild
{
    [Message(GameMessageOpcode.ServerGuildRename)]
    public class ServerGuildRename : IWritable
    {
        public Identity GuildIdentity { get; set; }
        public string Name { get; set; }

        public void Write(GamePacketWriter writer)
        {
            GuildIdentity.Write(writer);
            writer.WriteStringWide(Name);
        }
    }
}
