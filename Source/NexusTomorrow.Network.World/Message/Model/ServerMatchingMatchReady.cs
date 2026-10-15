using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingMatchReady, Direction.ServerToClient)]

// Specifically sent for a match that has not started yet
public sealed partial class ServerMatchingMatchReady
{
    [UInt(5)] public Game.Static.Matching.MatchType MatchType { get; set; }
    [UInt(32)] public uint PendingAllies { get; set; }
    [UInt(32)] public uint PendingEnemies { get; set; }
}
