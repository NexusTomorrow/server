using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.MultiHeal)]
public sealed partial class CombatLogMultiHeal : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.MultiHeal;

    [UInt(32)] public uint HealAmount { get; set; }
    [UInt(32)] public uint Overheal { get; set; }
    [UInt(32)] public uint Absorption { get; set; }
    [UInt(8)] public SpellEffectType EffectType { get; set; } // 8u
    public CombatLogCastData CastData { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(HealAmount);
        writer.Write(Overheal);
        writer.Write(Absorption);
        writer.Write(EffectType, 8u);
        CastData.Write(writer);
    }
}
