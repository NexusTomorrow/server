using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerRecruitmentGuildsUpdate, Direction.ServerToClient)]
public sealed partial class ServerRecruitmentGuildsUpdate
{
    /// <summary>
    /// The fields of a <see cref="RecruitmentGuildInfo"/> this message carries: all but its recruiters.
    /// </summary>
    [WireStruct]
    public sealed partial class RecruitmentGuild
    {
        [UInt(64)] public ulong GuildId { get; set; }
        [WideString] public string GuildName { get; set; } = "";
        [WideString] public string GuildMasterName { get; set; } = "";
        public GuildStats Stats { get; set; } = new();
    }

    [Array(CountBits = 32)] public List<RecruitmentGuild> Guilds { get; set; } = [];
}
