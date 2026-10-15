using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class GroupCharacter : IWritable
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
    public sealed partial class UnknownStruct1 : IWritable
    {
        [UInt(15)] public ushort Unknown30 { get; set; }
        [UInt(16)] public ushort Unknown31 { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Unknown30, 15u);
            writer.Write(Unknown31);
        }
    }

    [WideString] public string Name { get; set; }
    [UInt(14)] public Faction Faction { get; set; }
    [UInt(8)] public Race Race { get; set; }
    [Pad(6)] public Pad RaceHigh { get; }
    [UInt(8)] public Class Class { get; set; }
    [Pad(6)] public Pad ClassHigh { get; }
    [UInt(2)] public Sex Sex { get; set; }
    [UInt(7)] public byte Level { get; set; }
    [UInt(7)] public byte EffectiveLevel { get; set; }
    [UInt(3)] public Game.Static.PlayerPath.Path Path { get; set; }
    [UInt(17)] public uint Unknown4 { get; set; }
    [UInt(16)] public ushort GroupMemberId { get; set; }

    [FixedArray(5)] public UnknownStruct0[] SomeStatList = [new(), new(), new(), new(), new()];

    public Identity MentoringTarget { get; set; } = new();

    [UInt(32)] public uint Unknown10 { get; set; }
    [UInt(16)] public ushort Unknown11 { get; set; }
    [UInt(16)] public ushort Unknown12 { get; set; }
    [UInt(16)] public ushort Unknown13 { get; set; }
    [UInt(16)] public ushort Unknown14 { get; set; }
    [UInt(16)] public ushort Unknown15 { get; set; }
    [UInt(16)] public ushort Unknown16 { get; set; }
    [UInt(16)] public ushort Unknown17 { get; set; }
    [UInt(16)] public ushort Unknown18 { get; set; }
    [UInt(16)] public ushort Unknown19 { get; set; }
    [UInt(16)] public ushort Unknown20 { get; set; }
    [UInt(16)] public ushort Unknown21 { get; set; }
    [UInt(16)] public ushort Unknown22 { get; set; }

    [UInt(14)] public ushort Realm { get; set; }
    [UInt(15)] public ushort WorldZoneId { get; set; }
    [UInt(32)] public uint MapId { get; set; }
    [UInt(32)] public uint PhaseId { get; set; } = 1;
    [UInt(1)] public bool SyncedToGroup { get; set; }

    [UInt(32)] public uint Unknown28 { get; set; }
    [UInt(32)] public uint Unknown29 { get; set; }

    [Array(CountBits = 32)] public List<UnknownStruct1> UnknownStruct1List { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.WriteStringWide(Name);
        writer.Write(Faction, 14u);
        writer.Write(Race, 14u);
        writer.Write(Class, 14u);
        writer.Write(Sex, 2u);
        writer.Write(Level, 7u);
        writer.Write(EffectiveLevel, 7u);
        writer.Write(Path, 3u);
        writer.Write(Unknown4, 17u);
        writer.Write(GroupMemberId);

        for (int i = 0; i < 5; ++i)
        {
            SomeStatList[i] = new UnknownStruct0();
            SomeStatList[i].Write(writer);
        }

        if (MentoringTarget == null)
        {
            writer.Write((ushort)0, 14u);
            writer.Write((ulong)0);
        }
        else
        {
            MentoringTarget.Write(writer);
        }

        writer.Write(Unknown10);
        writer.Write(Unknown11);
        writer.Write(Unknown12);
        writer.Write(Unknown13);
        writer.Write(Unknown14);
        writer.Write(Unknown15);
        writer.Write(Unknown16);
        writer.Write(Unknown17);
        writer.Write(Unknown18);
        writer.Write(Unknown19);
        writer.Write(Unknown20);
        writer.Write(Unknown21);
        writer.Write(Unknown22);
        writer.Write(Realm, 14u);
        writer.Write(WorldZoneId, 15u);
        writer.Write(MapId);
        writer.Write(PhaseId);
        writer.Write(SyncedToGroup);
        writer.Write(Unknown28);
        writer.Write(Unknown29);

        writer.Write(UnknownStruct1List.Count);
        UnknownStruct1List.ForEach(i => i.Write(writer));
    }
}
