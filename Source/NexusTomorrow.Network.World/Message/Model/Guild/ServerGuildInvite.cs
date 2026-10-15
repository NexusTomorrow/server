using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild
{
    [Message(GameMessageOpcode.ServerGuildInvite)]
    public class ServerGuildInvite : IWritable
    {
        public string InviterName { get; set; }
        public string GuildName { get; set; }
        public GuildFlag Flags { get; set; }
        public GuildType GuildType { get; set; }
        public ulong Unused { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.WriteStringWide(InviterName);
            writer.WriteStringWide(GuildName);
            writer.Write(Flags, 32u);
            writer.Write(GuildType, 4u);
            writer.Write(Unused);
        }
    }
}
