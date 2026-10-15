using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.Transference)]
public sealed partial class CombatLogTransference : ICombatLog
{
    [WireStruct]
    public sealed partial class CombatHealData : IWritable
    {
        [UInt(32)] public uint HealedUnitId { get; set; }
        [UInt(32)] public uint HealAmount { get; set; }
        [UInt(5)] public Vital Vital { get; set; }
        [UInt(32)] public uint Overheal { get; set; }
        [UInt(32)] public uint Absorption { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(HealedUnitId);
            writer.Write(HealAmount);
            writer.Write(Vital, 5u);
            writer.Write(Overheal);
            writer.Write(Absorption);
        }
    }


    [NotOnWire] public CombatLogType Type => CombatLogType.Transference;

    [UInt(32)] public uint DamageAmount { get; set; }
    [UInt(3)] public DamageType DamageType { get; set; } // 3u
    [UInt(32)] public uint Shield { get; set; }
    [UInt(32)] public uint Absorption { get; set; }
    [UInt(32)] public uint Overkill { get; set; }
    [UInt(32)] public uint GlanceAmount { get; set; }
    [UInt(1)] public bool BTargetVulnerable { get; set; }
    [Array(CountBits = 32)] public List<CombatHealData> HealedUnits { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(DamageAmount);
        writer.Write(DamageType, 3u);
        writer.Write(Shield);
        writer.Write(Absorption);
        writer.Write(Overkill);
        writer.Write(GlanceAmount);
        writer.Write(BTargetVulnerable);
        writer.Write(HealedUnits.Count);

        HealedUnits.ForEach(unit => unit.Write(writer));
    }
}
