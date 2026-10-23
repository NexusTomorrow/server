using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using MatchType = NexusTomorrow.Game.Static.Matching.MatchType;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Sets the average wait of one matching queue; for an entry not in its match the client also raises MatchingAverageWaitTimeUpdated.
/// </summary>
[WireMessage(GameMessageOpcode.Server0628, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchingAverageWaitTime
{
    /// <summary>
    /// The match type of the queue, the one its entry is filed under.
    /// </summary>
    [UInt(5)] public MatchType MatchType { get; set; }

    /// <summary>
    /// The queue's average wait, in milliseconds.
    /// </summary>
    [UInt(32)] public uint AverageWaitTimeMs { get; set; }
}
