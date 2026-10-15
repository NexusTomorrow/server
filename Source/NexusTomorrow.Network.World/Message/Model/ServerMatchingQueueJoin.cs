using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingQueueJoin, Direction.ServerToClient)]
public sealed partial class ServerMatchingQueueJoin
{
    [WireStruct]
    public sealed partial class Map
    {
        [UInt(5)] public Game.Static.Matching.MatchType MatchType { get; set; }
        [Array(CountBits = 32), UInt(32)] public List<uint> MatchingGameMapIds { get; set; } = [];
        [UInt(14)] public ushort MatchingGameType { get; set; }
        [UInt(32)] public MatchingQueueFlags QueueFlags { get; set; }
    }

    [WireStruct]
    public sealed partial class Queue
    {
        [UInt(5)] public Game.Static.Matching.MatchType MatchType { get; set; }
        [UInt(1)] public bool IsParty { get; set; }
        [UInt(32)] public uint QueueTime { get; set; }
        [UInt(32)] public uint AverageWaitTime { get; set; }
    }

    public Map MapData { get; set; }
    public Queue QueueData { get; set; }
    [UInt(32)] public Role QueuedRoles { get; set; }
}
