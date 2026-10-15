using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.Damage)]
public partial class CombatLogDamage : ICombatLog
{
    [NotOnWire] public virtual CombatLogType Type => CombatLogType.Damage;

    [UInt(32)] public uint MitigatedDamage { get; set; }
    [UInt(32)] public uint RawDamage { get; set; }
    [UInt(32)] public uint Shield { get; set; }
    [UInt(32)] public uint Absorption { get; set; }
    [UInt(32)] public uint Overkill { get; set; }
    [UInt(32)] public uint Glance { get; set; }
    [UInt(1)] public bool BTargetVulnerable { get; set; }
    [UInt(1)] public bool BKilled { get; set; }
    [UInt(1)] public bool BPeriodic { get; set; }
    [UInt(3)] public DamageType DamageType { get; set; } // 3u
    [UInt(8)] public SpellEffectType EffectType { get; set; } // 8u
    public CombatLogCastData CastData { get; set; }
}
