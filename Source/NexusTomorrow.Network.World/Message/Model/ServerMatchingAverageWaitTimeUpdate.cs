using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Updates the wait time for a specific match type
[WireMessage(GameMessageOpcode.ServerMatchingAverageWaitTimeUpdate, Direction.ServerToClient)]
public sealed partial class ServerMatchingAverageWaitTimeUpdate
{
    [UInt(5)] public Game.Static.Matching.MatchType Type { get; set; }
    [UInt(32)] public uint AverageWaitTime { get; set; }
}
