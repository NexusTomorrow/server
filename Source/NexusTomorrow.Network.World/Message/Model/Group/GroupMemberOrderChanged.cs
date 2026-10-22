using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Group;

/// <summary>
/// Sets the order of a group's members.
/// </summary>
/// <remarks>
/// For its current group the client then raises Group_MemberOrderChanged.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0438, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GroupMemberOrderChanged
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
    /// How many members the two lists hold.
    /// </summary>
    [UInt(32), Count] public uint MemberCount { get; set; }

    /// <summary>
    /// The members, by id.
    /// </summary>
    [Array(CountFrom = nameof(MemberCount))] public List<RealmScopedId> MemberIds { get; } = [];

    /// <summary>
    /// Each member's order, in the same position; GroupLib.GetGroupMember reports it plus one as nOrder.
    /// </summary>
    [Array(CountFrom = nameof(MemberCount)), UInt(32)] public List<uint> Orders { get; } = [];
}
