using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 10: the target's death delayed.
/// </summary>
[WireStruct]
[UnionCase(10)]
public sealed partial class CombatLogDelayDeath : ICombatLogPayload
{
    /// <summary>
    /// The caster, target, spell and combat result.
    /// </summary>
    public CombatLogAction Action { get; set; } = new();
}
