using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 5: damage reflected.
/// </summary>
[WireStruct]
[UnionCase(5)]
public sealed partial class CombatLogReflect : ICombatLogPayload
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

    [UInt(32)] public uint Unknown4 { get; set; }

    /// <summary>
    /// The damage a glancing blow took off.
    /// </summary>
    [UInt(32)] public uint GlanceAmount { get; set; }

    /// <summary>
    /// Whether the damage killed the target.
    /// </summary>
    [UInt(1)] public bool TargetKilled { get; set; }

    [UInt(1)] public bool Unknown7 { get; set; }

    /// <summary>
    /// The damage type.
    /// </summary>
    [UInt(3)] public byte DamageType { get; set; }

    [UInt(8)] public byte Unknown9 { get; set; }

    /// <summary>
    /// The caster, target, spell and combat result.
    /// </summary>
    public CombatLogAction Action { get; set; } = new();
}
