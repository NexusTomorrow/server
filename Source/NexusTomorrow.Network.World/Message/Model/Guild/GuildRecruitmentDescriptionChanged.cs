using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets a recruiting guild's description.
/// </summary>
/// <remarks>
/// GuildTypeLib's GetRecruitmentDescription returns it; the client raises RecruitmentDescription.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0769, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildRecruitmentDescriptionChanged
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The guild's recruitment description.
    /// </summary>
    [WideString] public string Description { get; set; } = "";
}
