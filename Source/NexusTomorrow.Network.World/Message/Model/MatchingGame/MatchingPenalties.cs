using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using MatchType = NexusTomorrow.Game.Static.Matching.MatchType;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Sets the player's queue penalties, one per match type; the client raises MatchingPenaltyUpdated.
/// </summary>
[WireMessage(GameMessageOpcode.Server05D9, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchingPenalties
{
    /// <summary>
    /// The player's queue penalty for each match type, indexed by <see cref="MatchType"/>: a duration in milliseconds, 0 for none.
    /// </summary>
    [FixedArray(16), UInt(32)] public uint[] Penalties { get; set; } = new uint[16];
}
