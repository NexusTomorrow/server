using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
public sealed partial class SpellEffectResult
{
    [WireStruct]
    public sealed partial class AdditionalEffectResult
    {
        [UInt(32)] public uint RawDamage { get; set; }
        [UInt(32)] public uint Amount { get; set; }
        [UInt(32)] public uint Overheal { get; set; }
        [UInt(32)] public uint Absorption { get; set; }
        [UInt(32)] public uint Shield { get; set; }
        [UInt(32)] public uint GlanceOrAbsorb { get; set; }
        [UInt(32)] public uint Overkill { get; set; }
        [UInt(3)] public CombatResult CombatResult { get; set; }
    }

    [UInt(32)] public uint RawDamage { get; set; }
    [UInt(32)] public uint RawScaledDamage { get; set; }
    [UInt(32)] public uint AbsorbedAmount { get; set; }
    [UInt(32)] public uint ShieldAbsorbAmount { get; set; }
    [UInt(32)] public uint AdjustedDamage { get; set; }
    [UInt(32)] public uint OverkillAmount { get; set; }
    [UInt(32)] public uint Glance { get; set; }
    [UInt(1)] public bool KilledTarget { get; set; }
    [UInt(4)] public CombatResult CombatResult { get; set; }
    [UInt(3)] public DamageType DamageType { get; set; }

    [Array(CountBits = 8)] public List<AdditionalEffectResult> AdditionaEffectResults { get; set; } = [];
}
