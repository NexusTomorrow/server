using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingQueueJoin)]
[WireMessage(GameMessageOpcode.ServerMatchingQueueJoin, Direction.ServerToClient)]
public sealed partial class ServerMatchingQueueJoin : IWritable
{
    [WireStruct]
    public sealed partial class Map : IWritable
    {
        [UInt(5)] public Game.Static.Matching.MatchType MatchType { get; set; }
        [Array(CountBits = 32), UInt(32)] public List<uint> MatchingGameMapIds { get; set; } = [];
        [UInt(14)] public ushort MatchingGameType { get; set; }
        [UInt(32)] public MatchingQueueFlags QueueFlags { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(MatchType, 5u);

            writer.Write(MatchingGameMapIds.Count);
            foreach (uint mapId in MatchingGameMapIds)
                writer.Write(mapId);

            writer.Write(MatchingGameType, 14u);
            writer.Write(QueueFlags, 32u);
        }
    }

    [WireStruct]
    public sealed partial class Queue : IWritable
    {
        [UInt(5)] public Game.Static.Matching.MatchType MatchType { get; set; }
        [UInt(1)] public bool IsParty { get; set; }
        [UInt(32)] public uint QueueTime { get; set; }
        [UInt(32)] public uint AverageWaitTime { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(MatchType, 5u);
            writer.Write(IsParty);
            writer.Write(QueueTime);
            writer.Write(AverageWaitTime);
        }
    }

    public Map MapData { get; set; }
    public Queue QueueData { get; set; }
    [UInt(32)] public Role QueuedRoles { get; set; }

    public void Write(GamePacketWriter writer)
    {
        MapData.Write(writer);
        QueueData.Write(writer);

        writer.Write(QueuedRoles, 32u);
    }
}
