using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server0767, Direction.ServerToClient)]
public sealed partial class GuildRecruitmentList
{
    [Array(CountBits = 32)] public List<GuildRecruitmentListing> Unknown0 { get; set; } = [];
}
