using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild
{
    [Message(GameMessageOpcode.ServerRecruitmentGuildDetails)]
    public class ServerRecruitmentGuildDetails : IWritable
    {
        public Identity GuildIdentity { get; set; }
        public string Description { get; set; }
        public float GuildCreationDaysAgo { get; set; }
        public bool HasTax { get; set; }
        public uint RecruitmentMinimumLevel { get; set; }
        public RecruitmentDemands Demands { get; set; }

        public void Write(GamePacketWriter writer)
        {
            GuildIdentity.Write(writer);
            writer.WriteStringWide(Description);
            writer.Write(GuildCreationDaysAgo);
            writer.Write(HasTax);
            writer.Write(RecruitmentMinimumLevel);
            Demands.Write(writer);
        }
    }
}
