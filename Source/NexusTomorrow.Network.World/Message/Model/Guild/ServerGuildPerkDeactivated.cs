using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild
{
    [Message(GameMessageOpcode.ServerGuildPerkDeactivated)]
    public class ServerGuildPerkDeactivated : IWritable
    {
        public Identity GuildIdentity { get; set; }
        public ushort GuildPerkId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            GuildIdentity.Write(writer);
            writer.Write(GuildPerkId, 14u);
        }
    }
}
