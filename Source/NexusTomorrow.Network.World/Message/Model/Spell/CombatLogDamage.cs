using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// The payload of the damage entries, plain, to shields and multi-hit alike: arms 3, 4, 6 and 7.
/// </summary>
[WireStruct]
public partial class CombatLogDamage : ICombatLogPayload
{
    /// <summary>
    /// The damage dealt.
    /// </summary>
    [UInt(32)] public uint DamageAmount { get; set; }

    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The damage the target's shields took.
    /// </summary>
    [UInt(32)] public uint Shield { get; set; }

    /// <summary>
    /// The damage absorbed.
    /// </summary>
    [UInt(32)] public uint Absorption { get; set; }

    /// <summary>
    /// The damage beyond what the target had left.
    /// </summary>
    [UInt(32)] public uint Overkill { get; set; }

    /// <summary>
    /// The damage a glancing blow took off.
    /// </summary>
    [UInt(32)] public uint GlanceAmount { get; set; }

    /// <summary>
    /// Whether the target was vulnerable.
    /// </summary>
    [UInt(1)] public bool TargetVulnerable { get; set; }

    /// <summary>
    /// Whether the damage killed the target.
    /// </summary>
    [UInt(1)] public bool TargetKilled { get; set; }

    /// <summary>
    /// Whether the damage is periodic.
    /// </summary>
    [UInt(1)] public bool Periodic { get; set; }

    /// <summary>
    /// The damage type.
    /// </summary>
    [UInt(3)] public byte DamageType { get; set; }

    /// <summary>
    /// The type of the spell effect that dealt the damage.
    /// </summary>
    [UInt(8)] public byte EffectType { get; set; }

    /// <summary>
    /// The caster, target, spell and combat result.
    /// </summary>
    public CombatLogAction Action { get; set; } = new();
}
