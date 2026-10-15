using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerRecruitmentGuildsList, Direction.ServerToClient)]
public sealed partial class ServerRecruitmentGuildsList
{
    [Array(CountBits = 32)] public List<RecruitmentGuildInfo> Guilds { get; set; }
}
