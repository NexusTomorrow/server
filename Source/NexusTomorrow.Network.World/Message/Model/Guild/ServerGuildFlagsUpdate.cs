using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild
{
    [Message(GameMessageOpcode.ServerGuildFlagsUpdate)]
    public class ServerGuildFlagsUpdate : IWritable
    {
        public Identity GuildIdentity { get; set; }
        public GuildFlag Flags { get; set; }

        public void Write(GamePacketWriter writer)
        {
            GuildIdentity.Write(writer);
            writer.Write(Flags, 32u);
        }
    }
}
