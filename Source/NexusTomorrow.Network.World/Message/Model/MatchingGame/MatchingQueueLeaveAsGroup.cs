using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using MatchType = NexusTomorrow.Game.Static.Matching.MatchType;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Leaves one matching queue the player joined as a group.
/// </summary>
[WireMessage(GameMessageOpcode.Client05B6, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class MatchingQueueLeaveAsGroup
{
    /// <summary>
    /// The match type of the queue to leave, the one its entry is filed under.
    /// </summary>
    [UInt(5)] public MatchType MatchType { get; private set; }
}
