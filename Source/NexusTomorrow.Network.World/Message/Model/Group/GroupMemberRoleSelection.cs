using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Updates a group member's matching role selection.
/// </summary>
/// <remarks>
/// The client looks up the member by realm-scoped id and records the role; a zero role clears it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0600, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupMemberRoleSelection
{
    /// <summary>
    /// The group member, by realm-scoped id.
    /// </summary>
    [VerifiedMember] public RealmScopedId UnitId { get; set; } = new();

    /// <summary>
    /// The role selection; when 0 the client clears the member's role.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Role { get; set; }
}