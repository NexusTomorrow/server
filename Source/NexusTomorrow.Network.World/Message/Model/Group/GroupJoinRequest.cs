using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Tells the player that someone asks to join the group.
/// </summary>
/// <remarks>
/// The client keeps it as the pending request, which <see cref="GroupRequestAnswer"/> answers, and raises Group_JoinRequest with
/// the requester's name.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0424, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GroupJoinRequest
{
    /// <summary>
    /// The request, by id; <see cref="GroupRequestAnswer"/> sends it back.
    /// </summary>
    [UInt(64)] public ulong RequestId { get; set; }

    /// <summary>
    /// The player asking to join.
    /// </summary>
    public GroupMemberRecord Requester { get; set; } = new();
}
