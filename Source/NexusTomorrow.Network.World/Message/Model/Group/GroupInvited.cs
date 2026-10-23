using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Invites the player to a group.
/// </summary>
/// <remarks>
/// The client keeps the invitation, which <see cref="GroupInviteAnswer"/> answers, and raises Group_Invited with the inviter's name.
/// </remarks>
[WireMessage(GameMessageOpcode.Server041F, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GroupInvited
{
    /// <summary>
    /// The invitation, by id; <see cref="GroupInviteAnswer"/> sends it back.
    /// </summary>
    [UInt(64)] public ulong InviteId { get; set; }

    /// <summary>
    /// The group's leader, as an index into <see cref="Members"/>; GroupLib.GetInvite reports that member as the leader.
    /// </summary>
    [UInt(32)] public uint LeaderIndex { get; set; }

    /// <summary>
    /// The player who sent the invitation, as an index into <see cref="Members"/>.
    /// </summary>
    [UInt(32)] public uint InviterIndex { get; set; }

    /// <summary>
    /// The group's members.
    /// </summary>
    [Array(CountBits = 32)] public List<GroupMemberInfo> Members { get; set; } = [];
}
