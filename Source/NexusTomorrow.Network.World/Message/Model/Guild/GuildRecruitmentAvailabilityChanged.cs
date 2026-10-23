using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets whether a guild member is available for recruitment.
/// </summary>
/// <remarks>
/// The client stores it only for the player's own member, where GuildTypeLib's GetMyRecruitmentAvailability returns it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server049F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildRecruitmentAvailabilityChanged
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The member, by character id.
    /// </summary>
    public RealmScopedId MemberId { get; set; } = new();

    /// <summary>
    /// The member's flags, by bit as <see cref="GuildMember.Flags"/>.
    /// </summary>
    [UInt(32)] public uint Flags { get; set; }
}
