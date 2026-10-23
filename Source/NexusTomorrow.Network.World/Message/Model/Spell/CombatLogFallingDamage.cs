using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 8: damage from a fall.
/// </summary>
[WireStruct]
[UnionCase(8)]
public sealed partial class CombatLogFallingDamage : ICombatLogPayload
{
    /// <summary>
    /// The caster, by unit id.
    /// </summary>
    [UInt(32)] public uint CasterId { get; set; }

    /// <summary>
    /// The damage dealt.
    /// </summary>
    [UInt(32)] public uint DamageAmount { get; set; }
}
