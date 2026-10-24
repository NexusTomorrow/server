using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Lists the guilds that are recruiting.
/// </summary>
/// <remarks>
/// The client replaces its list and raises RecruitmentGuildsList.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0767, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GuildRecruitmentList
{
    /// <summary>
    /// The recruiting guilds.
    /// </summary>
    [Array(CountBits = 32)] public List<GuildRecruitmentListing> Guilds { get; set; } = [];
}
