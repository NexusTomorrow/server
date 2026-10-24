using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class Server0262Part10
{
    [WireStruct]
    public sealed partial class Server0262Part11
    {
        [UInt(32)] public uint Unknown0 { get; set; }

        /// <summary>
        /// The amount the hit dealt or healed, which the combat log reports as nDamageAmount or nHealAmount.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint Amount { get; set; }

        [UInt(32)] public uint Unknown2 { get; set; }

        /// <summary>
        /// The amount absorbed from the hit, which the combat log reports as nAbsorption.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint Absorption { get; set; }

        /// <summary>
        /// The amount of the hit the target's shields took, which the combat log reports as nShield.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint Shield { get; set; }

        /// <summary>
        /// The amount a glancing blow took off the hit, which the combat log reports as nGlanceAmount.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint GlanceAmount { get; set; }

        /// <summary>
        /// The hit's damage beyond what the target had left, which the combat log reports as nOverkill.
        /// </summary>
        [VerifiedMember, UInt(32)] public uint Overkill { get; set; }

        [UInt(3)] public byte Unknown7 { get; set; }
    }

    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The amount absorbed, which the combat log reports as nAbsorption.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Absorption { get; set; }

    /// <summary>
    /// The amount the target's shields took, which the combat log reports as nShield.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Shield { get; set; }

    /// <summary>
    /// The amount dealt or healed. The combat log reports it as nHealAmount for a heal and as nDamageAmount for damage to shields, and
    /// reports other damage as this amount less <see cref="Excess"/>.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Amount { get; set; }

    /// <summary>
    /// The amount beyond what the target had left, which the combat log reports as nOverkill for damage and as nOverheal for a heal.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Excess { get; set; }

    /// <summary>
    /// The amount a glancing blow took off, which the combat log reports as nGlanceAmount.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint GlanceAmount { get; set; }

    /// <summary>
    /// Whether the effect killed the target, which the combat log reports as bTargetKilled.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool TargetKilled { get; set; }

    /// <summary>
    /// How the effect landed, which the combat log reports as eCombatResult. The client reports no damage or heal entry for the effect
    /// when it is 0.
    /// </summary>
    [VerifiedMember, UInt(4)] public CombatResult CombatResult { get; set; }

    /// <summary>
    /// The damage type, which the combat log reports as eDamageType.
    /// </summary>
    [VerifiedMember, UInt(3)] public DamageType DamageType { get; set; }

    /// <summary>
    /// The effect's further hits. The client reads only the first.
    /// </summary>
    [VerifiedMember, Array(CountBits = 8)] public List<Server0262Part11> MultiHits { get; set; } = [];
}
