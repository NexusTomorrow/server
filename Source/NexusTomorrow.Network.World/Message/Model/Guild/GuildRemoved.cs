using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Takes the player out of a guild.
/// </summary>
/// <remarks>
/// The client leaves the guild's chat and drops the guild.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0495, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildRemoved
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();
}
