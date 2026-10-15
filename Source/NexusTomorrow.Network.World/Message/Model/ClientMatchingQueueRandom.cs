using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientMatchingQueueRandom, Direction.ClientToServer)]
public sealed partial class ClientMatchingQueueRandom
{
    [UInt(5)] public Game.Static.Matching.MatchType MatchType { get; private set; }
    [UInt(32)] public MatchingQueueFlags Flags { get; private set; }
    [UInt(32)] public Role Roles { get; private set; }
}
