using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.MultiHit)]
public sealed partial class CombatLogMultiHit : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.MultiHit;

    [UInt(32)] public uint DamageAmount { get; set; }
    [UInt(32)] public uint RawDamage { get; set; }
    [UInt(32)] public uint Shield { get; set; }
    [UInt(32)] public uint Absorption { get; set; }
    [UInt(32)] public uint Overkill { get; set; }
    [UInt(32)] public uint GlanceAmount { get; set; }
    [UInt(1)] public bool BTargetVulnerable { get; set; }
    [UInt(1)] public bool BKilled { get; set; }
    [UInt(1)] public bool BPeriodic { get; set; }
    [UInt(3)] public DamageType DamageType { get; set; } // 3u
    [UInt(8)] public SpellEffectType EffectType { get; set; } // 8u
    public CombatLogCastData CastData { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(DamageAmount);
        writer.Write(RawDamage);
        writer.Write(Shield);
        writer.Write(Absorption);
        writer.Write(Overkill);
        writer.Write(GlanceAmount);
        writer.Write(BTargetVulnerable);
        writer.Write(BKilled);
        writer.Write(BPeriodic);
        writer.Write(DamageType, 3u);
        writer.Write(EffectType, 8u);
        CastData.Write(writer);
    }
}
