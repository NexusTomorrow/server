using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 17: an attack deflected.
/// </summary>
[WireStruct]
[UnionCase(17)]
public sealed partial class CombatLogDeflect : ICombatLogPayload
{
    /// <summary>
    /// Whether the attack deflected was a multi-hit.
    /// </summary>
    [UInt(1)] public bool MultiHit { get; set; }

    /// <summary>
    /// The caster, target, spell and combat result.
    /// </summary>
    public CombatLogAction Action { get; set; } = new();
}
