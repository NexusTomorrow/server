using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using MatchType = NexusTomorrow.Game.Static.Matching.MatchType;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Tells the player a match is ready for them to accept.
/// </summary>
/// <remarks>
/// The client stores it as the pending match, starts the time the player has to answer and raises MatchingGameReady for a new match.
/// </remarks>
[WireMessage(GameMessageOpcode.Server05CA, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchReady
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
    /// How many enemies have yet to accept.
    /// </summary>
    [UInt(32)] public uint PendingEnemies { get; set; }
}
