using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Reputation;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class GroupCharacter
{
    [WireStruct]
    public sealed partial class UnknownStruct0
    {
        [UInt(16)] public ushort Unknown6 { get; set; } = 0;
        [UInt(8)] public byte Unknown7 { get; set; } = 48;
    }

    [WireStruct]
    public sealed partial class UnknownStruct1
    {
        [UInt(15)] public ushort Unknown30 { get; set; }
        [UInt(16)] public ushort Unknown31 { get; set; }
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
}
