using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingQueueStatus, Direction.ServerToClient)]
public sealed partial class ServerMatchingQueueStatus
{
    [UInt(4)] public MatchingQueueStatus Status { get; set; }
    [UInt(5)] public Game.Static.Matching.MatchType JoinedMatchType { get; set; }
    [UInt(5)] public Game.Static.Matching.MatchType ReadyMatchType { get; set; } // MatchType for active role check on group that members can role check for
    [FixedArray(2), UInt(8)] public byte[] QueuesJoined { get; set; } = new byte[2];
}
