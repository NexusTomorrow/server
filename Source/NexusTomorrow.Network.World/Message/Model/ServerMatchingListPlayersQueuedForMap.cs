using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingListPlayersQueuedForMap, Direction.ServerToClient)]
public sealed partial class ServerMatchingListPlayersQueuedForMap
{
    [WireStruct]
    public sealed partial class UnknownStruct0
    {
        [UInt(16)] public ushort Unknown6 { get; set; } = 0;
        [UInt(8)] public byte Unknown7 { get; set; } = 48;
    }

    [WireStruct]
    public sealed partial class PrimeLevelInfo
    {
        [UInt(15)] public ushort WorldId { get; set; }
        [UInt(16)] public ushort PrimeLevelAchieved { get; set; }
    }

    [WireStruct]
    public sealed partial class QueuedPlayerInfo
    {
        public Identity Identity { get; set; }
        [WideString] public string Name { get; set; }
        [UInt(14)] public Faction Faction { get; set; }
        [UInt(14)] public uint Race { get; set; }
        [UInt(14)] public uint Class { get; set; }
        [UInt(14)] public uint Gender { get; set; }
        [UInt(32)] public uint Level { get; set; }
        [UInt(3)] public uint Path { get; set; }
        [UInt(32)] public uint field_30_32bit { get; set; }
        [UInt(1)] public bool field_34_1bit { get; set; }
        [UInt(32)] public uint field_38_32bit { get; set; }
        [Float32] public float field_3C_32bit { get; set; }
        [FixedArray(5)] public UnknownStruct0[] SomeStatList = new UnknownStruct0[5];
        [Array(CountBits = 32)] public List<PrimeLevelInfo> PrimeLevels { get; set; } = [];

    }

    [UInt(32)] public uint MapId { get; set; }
    [Array(CountBits = 32)] public List<QueuedPlayerInfo> QueuedPlayers { get; set; } = [];
}
