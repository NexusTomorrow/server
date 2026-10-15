using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
public sealed partial class SpellEffectResult : IWritable
{
    [WireStruct]
    public sealed partial class AdditionalEffectResult : IWritable
    {
        [UInt(32)] public uint RawDamage { get; set; }
        [UInt(32)] public uint Amount { get; set; }
        [UInt(32)] public uint Overheal { get; set; }
        [UInt(32)] public uint Absorption { get; set; }
        [UInt(32)] public uint Shield { get; set; }
        [UInt(32)] public uint GlanceOrAbsorb { get; set; }
        [UInt(32)] public uint Overkill { get; set; }
        [UInt(3)] public CombatResult CombatResult { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(RawDamage);
            writer.Write(Amount);
            writer.Write(Overheal);
            writer.Write(Absorption);
            writer.Write(Shield);
            writer.Write(GlanceOrAbsorb);
            writer.Write(Overkill);
            writer.Write(CombatResult, 3u);
        }
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

    public void Write(GamePacketWriter writer)
    {
        writer.Write(RawDamage);
        writer.Write(RawScaledDamage);
        writer.Write(AbsorbedAmount);
        writer.Write(ShieldAbsorbAmount);
        writer.Write(AdjustedDamage);
        writer.Write(OverkillAmount);
        writer.Write(Glance);
        writer.Write(KilledTarget);
        writer.Write(CombatResult, 4u);
        writer.Write(DamageType, 3u);

        writer.Write(AdditionaEffectResults.Count, 8u);
        AdditionaEffectResults.ForEach(u => u.Write(writer));
    }
}
