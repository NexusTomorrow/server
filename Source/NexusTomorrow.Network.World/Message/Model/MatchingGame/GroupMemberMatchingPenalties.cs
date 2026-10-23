using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;
using MatchType = NexusTomorrow.Game.Static.Matching.MatchType;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Sets a group member's queue penalties, one per match type; the client raises MatchingPenaltyUpdated.
/// </summary>
[WireMessage(GameMessageOpcode.Server0436, Direction.ServerToClient)]
[Verified(Rung.Fields)]
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
    /// The member's queue penalty for each match type, indexed by <see cref="MatchType"/>: a duration in milliseconds, 0 for none.
    /// </summary>
    [FixedArray(16), UInt(32)] public uint[] Penalties { get; set; } = new uint[16];
}
