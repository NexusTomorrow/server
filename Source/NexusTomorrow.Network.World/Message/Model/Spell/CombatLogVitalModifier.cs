using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 16: a vital changed.
/// </summary>
[WireStruct]
[UnionCase(16)]
public sealed partial class CombatLogVitalModifier : ICombatLogPayload
{
    /// <summary>
    /// How much the vital changed.
    /// </summary>
    [Float32] public float Amount { get; set; }

    /// <summary>
    /// The vital.
    /// </summary>
    [UInt(5)] public byte VitalType { get; set; }

    /// <summary>
    /// Whether the combat log shows the entry.
    /// </summary>
    [UInt(1)] public bool ShowCombatLog { get; set; }

    /// <summary>
    /// The caster, target, spell and combat result.
    /// </summary>
    public CombatLogAction Action { get; set; } = new();
}
