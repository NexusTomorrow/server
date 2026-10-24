using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Gives the positions of the player's group members; the client raises Group_UpdatePosition.
/// </summary>
[WireMessage(GameMessageOpcode.Server0469, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GroupMemberPositions
{
    /// <summary>
    /// The group, which the client compares with the player's own; it ignores the message for another group.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The members' world, as a World id.
    /// </summary>
    [UInt(15)] public uint WorldId { get; set; }

    /// <summary>
    /// How many members the four lists hold.
    /// </summary>
    [UInt(32), Count] public uint MemberCount { get; set; }

    /// <summary>
    /// The members, by realm-scoped character id.
    /// </summary>
    [Array(CountFrom = nameof(MemberCount))] public List<RealmScopedId> MemberIds { get; set; } = [];

    /// <summary>
    /// Each member's position.
    /// </summary>
    [Array(CountFrom = nameof(MemberCount))] public List<Client0096Part0> Positions { get; set; } = [];

    /// <summary>
    /// Each member's zone, which the client uses for a member with no unit in view.
    /// </summary>
    [Array(CountFrom = nameof(MemberCount)), UInt(32)] public List<uint> ZoneIds { get; set; } = [];

    /// <summary>
    /// Each member's combat state, as a number: bit 0 is set while the member is in PvP combat; bits 2 to 31 the client never reads.
    /// </summary>
    [Array(CountFrom = nameof(MemberCount)), UInt(32)] public List<uint> CombatFlags { get; set; } = [];
}
