using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingQueueResultAnnounce, Direction.ServerToClient)]
public sealed partial class ServerMatchingQueueResultAnnounce
{
    [UInt(6)] public MatchingQueueResult Result { get; set; }
    [UInt(4)] public MatchingQueueStatus Status { get; set; }
}
