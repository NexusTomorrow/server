using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sends a value for a recruiting guild.
/// </summary>
/// <remarks>
/// The client raises RecruitmentDescription.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0769, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GuildRecruitmentDescriptionChanged
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    [WideString] public string Unknown1 { get; set; } = "";
}
