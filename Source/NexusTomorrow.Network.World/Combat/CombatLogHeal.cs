using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.Heal)]
public sealed partial class CombatLogHeal : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.Heal;

    [UInt(32)] public uint HealAmount { get; set; }
    [UInt(32)] public uint Overheal { get; set; }
    [UInt(32)] public uint Absorption { get; set; } // not used by client
    [UInt(8)] public SpellEffectType EffectType { get; set; } // 8u
    public CombatLogCastData CastData { get; set; }
}
