using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientMatchingQueueLeave, Direction.ClientToServer)]
public sealed partial class ClientMatchingQueueLeave
{
    [UInt(5)] public Game.Static.Matching.MatchType MatchType { get; private set; }
}
