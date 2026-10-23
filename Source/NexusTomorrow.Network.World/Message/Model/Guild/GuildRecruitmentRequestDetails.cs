using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Asks for a recruiting guild's details.
/// </summary>
/// <remarks>
/// The client sends it at most once a second.
/// </remarks>
[WireMessage(GameMessageOpcode.Client076E, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class GuildRecruitmentRequestDetails
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; private set; } = new();
}
