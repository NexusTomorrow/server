using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerRecruitmentGuildsRecruiters)]
[WireMessage(GameMessageOpcode.ServerRecruitmentGuildsRecruiters, Direction.ServerToClient)]
public sealed partial class ServerRecruitmentGuildsRecruiters : IWritable
{
    [WireStruct]
    public sealed partial class GuildRecruiters : IWritable
    {
        [UInt(64)] public ulong GuildId { get; set; }
        [UInt(1)] public bool IsRecruiting { get; set; }
        [UInt(32), Count] public uint RecruiterNamesCount { get; set; }
        [Array(CountFrom = nameof(RecruiterNamesCount)), WideString] public List<string> RecruiterNames { get; set; } = [];
        [Array(CountFrom = nameof(RecruiterNamesCount)), UInt(1)] public List<bool> IsOnline { get; set; } = []; // isOnline state of the recruiter

        public void Write(GamePacketWriter writer)
        {
            writer.Write(GuildId);
            writer.Write(IsRecruiting);

            writer.Write(RecruiterNames.Count);
            RecruiterNames.ForEach(s => writer.WriteStringWide(s));
            IsOnline.ForEach(b => writer.Write(b));
        }
    }

    [Array(CountBits = 32)] public List<GuildRecruiters> RecruitingGuilds { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(RecruitingGuilds.Count);
        RecruitingGuilds.ForEach(guild => guild.Write(writer));
    }
}
