using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingMatchInProgressReady, Direction.ServerToClient)]

// If player is in a match, fires MatchingGamePendingUpdate event for ally count update
// If player is not in a match, fires MatchingGameReady event for a match that is not started
public sealed partial class ServerMatchingMatchInProgressReady
{
    [UInt(5)] public Game.Static.Matching.MatchType MatchType { get; set; }
    [UInt(32)] public uint PendingAllies { get; set; }
    [UInt(32)] public uint CurrentAllies { get; set; }
}
