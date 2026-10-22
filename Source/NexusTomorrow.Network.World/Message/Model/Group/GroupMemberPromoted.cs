using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Makes a member the group's leader.
/// </summary>
/// <remarks>
/// The client clears the old leader's mark and marks the new one; for its current group it raises Group_MemberPromoted with
/// the new leader's name and whether that is the player.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0447, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupMemberPromoted
{
    /// <summary>
    /// The group, by id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The new leader.
    /// </summary>
    public RealmScopedId LeaderId { get; set; } = new();
}
