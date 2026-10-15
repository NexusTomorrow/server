using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

public class SpellEffectResult : IWritable
{
    public class AdditionalEffectResult : IWritable
    {
        public uint RawDamage { get; set; }
        public uint Amount { get; set; }
        public uint Overheal { get; set; }
        public uint Absorption { get; set; }
        public uint Shield { get; set; }
        public uint GlanceOrAbsorb { get; set; }
        public uint Overkill { get; set; }
        public CombatResult CombatResult { get; set; }

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

    public uint RawDamage { get; set; }
    public uint RawScaledDamage { get; set; }
    public uint AbsorbedAmount { get; set; }
    public uint ShieldAbsorbAmount { get; set; }
    public uint AdjustedDamage { get; set; }
    public uint OverkillAmount { get; set; }
    public uint Glance { get; set; }
    public bool KilledTarget { get; set; }
    public CombatResult CombatResult { get; set; }
    public DamageType DamageType { get; set; }

    public List<AdditionalEffectResult> AdditionaEffectResults { get; set; } = [];

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
