using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sends members of a guild's roster.
/// </summary>
[WireMessage(GameMessageOpcode.Server04A0, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GuildRoster
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The members.
    /// </summary>
    [Array(CountBits = 32)] public List<GuildMember> Members { get; set; } = [];

    /// <summary>
    /// True when the roster is complete: the client raises GuildRoster and no longer asks for the members.
    /// </summary>
    [UInt(1)] public bool Complete { get; set; }
}
