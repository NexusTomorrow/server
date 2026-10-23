using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Sets a group member's matching penalties; the client raises MatchingPenaltyUpdated.
/// </summary>
[WireMessage(GameMessageOpcode.Server0436, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GroupMemberMatchingPenalties
{
    /// <summary>
    /// The group, by group id.
    /// </summary>
    [UInt(64)] public ulong GroupId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The member, by realm-scoped character id.
    /// </summary>
    public RealmScopedId MemberId { get; set; } = new();

    /// <summary>
    /// The member's sixteen penalty values, as <see cref="MatchingPenalties.Penalties"/> carries the player's.
    /// </summary>
    [FixedArray(16), UInt(32)] public uint[] Penalties { get; set; } = new uint[16];
}
