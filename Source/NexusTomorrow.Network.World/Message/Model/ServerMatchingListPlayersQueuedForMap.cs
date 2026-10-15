using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingListPlayersQueuedForMap)]
[WireMessage(GameMessageOpcode.ServerMatchingListPlayersQueuedForMap, Direction.ServerToClient)]
public sealed partial class ServerMatchingListPlayersQueuedForMap : IWritable
{
    [WireStruct]
    public sealed partial class UnknownStruct0 : IWritable
    {
        [UInt(16)] public ushort Unknown6 { get; set; } = 0;
        [UInt(8)] public byte Unknown7 { get; set; } = 48;

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Unknown6);
            writer.Write(Unknown7);
        }
    }

    [WireStruct]
    public sealed partial class PrimeLevelInfo : IWritable
    {
        [UInt(15)] public ushort WorldId { get; set; }
        [UInt(16)] public ushort PrimeLevelAchieved { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(WorldId, 15u);
            writer.Write(PrimeLevelAchieved);
        }
    }

    [WireStruct]
    public sealed partial class QueuedPlayerInfo : IWritable
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

        public void Write(GamePacketWriter writer)
        {
            Identity.Write(writer);
            writer.WriteStringWide(Name);
            writer.Write(Faction, 14u);
            writer.Write(Race, 14u);
            writer.Write(Class, 14u);
            writer.Write(Gender, 14u);
            writer.Write(Level);
            writer.Write(Path, 3u);
            writer.Write(field_30_32bit);
            writer.Write(field_34_1bit);
            writer.Write(field_38_32bit);
            writer.Write(field_3C_32bit);
            foreach (UnknownStruct0 stat in SomeStatList)
            {
                stat.Write(writer);
            }
            writer.Write(PrimeLevels.Count);
            PrimeLevels.ForEach(i => i.Write(writer));
        }

    }

    [UInt(32)] public uint MapId { get; set; }
    [Array(CountBits = 32)] public List<QueuedPlayerInfo> QueuedPlayers { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(MapId);
        writer.Write(QueuedPlayers.Count);
        foreach (QueuedPlayerInfo player in QueuedPlayers)
        {
            player.Write(writer);
        }
    }
}
