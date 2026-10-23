using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets what a recruiting guild asks for.
/// </summary>
/// <remarks>
/// The client raises RecruitmentDemands.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0768, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildRecruitmentDemandsChanged
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// What the guild asks for.
    /// </summary>
    public GuildRecruitmentDemands Demands { get; set; } = new();
}
