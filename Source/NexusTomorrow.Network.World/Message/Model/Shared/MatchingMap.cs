using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class MatchingMap
{
    [UInt(5)] public Game.Static.Matching.MatchType MatchType { get; private set; }
    [Array(CountBits = 32), UInt(32)] public List<uint> Maps { get; } = []; // MatchingGameMapIds
    [UInt(14)] public ushort MatchingGameTypeId { get; private set; }
    [UInt(32)] public MatchingQueueFlags QueueFlags { get; private set; }

    public void Read(GamePacketReader reader)
    {
        MatchType = reader.ReadEnum<Game.Static.Matching.MatchType>(5u);
        uint mapCount = reader.ReadUInt();

        for (uint i = 0; i < mapCount; i++)
            Maps.Add(reader.ReadUInt());

        MatchingGameTypeId = reader.ReadUShort(14u);
        QueueFlags = reader.ReadEnum<MatchingQueueFlags>(32u);
    }
}
