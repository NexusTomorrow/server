using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using MatchType = NexusTomorrow.Game.Static.Matching.MatchType;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Offers the player a place in a match already under way, with the allies' answers so far.
/// </summary>
/// <remarks>
/// The client stores it as the pending match and raises MatchingGameReady for a match in progress, or MatchingGamePendingUpdate when
/// the player is already in the match.
/// </remarks>
[WireMessage(GameMessageOpcode.Server05C4, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchReadyInProgress
{
    /// <summary>
    /// The match's match type.
    /// </summary>
    [UInt(5)] public MatchType MatchType { get; set; }

    /// <summary>
    /// How many allies have yet to accept.
    /// </summary>
    [UInt(32)] public uint PendingAllies { get; set; }

    /// <summary>
    /// How many allies have accepted.
    /// </summary>
    [UInt(32)] public uint AcceptedAllies { get; set; }
}
