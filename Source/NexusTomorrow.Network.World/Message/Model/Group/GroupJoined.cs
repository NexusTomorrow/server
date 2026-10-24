using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Puts the player in a group: the group's members, rules and target marks.
/// </summary>
/// <remarks>
/// The client refuses a group whose id is 0, or whose maximum size is 0 or below its member count. A group flagged as an instance group
/// goes to the instance group's place while the player already has a group.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0427, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GroupJoined
{
    /// <summary>
    /// The receiving player's own member id in the group.
    /// </summary>
    [VerifiedMember] public RealmScopedId PlayerId { get; set; } = new();

    /// <summary>
    /// The group joined.
    /// </summary>
    [VerifiedMember] public GroupRecord Group { get; set; } = new GroupRecord();
}
