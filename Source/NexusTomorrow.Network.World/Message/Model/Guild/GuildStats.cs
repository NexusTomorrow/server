using NexusTomorrow.Game.Static.Guild;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild
{
    public class GuildStats : IWritable
    {
        public ushort MemberCount { get; set; }
        public ushort PerkCount { get; set; }
        public GuildClassification Classification { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(MemberCount);
            writer.Write(PerkCount);
            writer.Write(Classification, 3u);
        }
    }
}
