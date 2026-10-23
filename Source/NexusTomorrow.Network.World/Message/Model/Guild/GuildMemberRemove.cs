using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Removes a member from a guild and sets its member counts.
/// </summary>
/// <remarks>
/// The client then raises GuildMemberChange.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04A2, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildMemberRemove
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The member the client removes.
    /// </summary>
    public RealmScopedId MemberId { get; set; } = new();

    /// <summary>
    /// The guild's member count.
    /// </summary>
    [UInt(16)] public ushort MemberCount { get; set; }

    /// <summary>
    /// How many of its members are online.
    /// </summary>
    [UInt(16)] public ushort OnlineMemberCount { get; set; }
}
