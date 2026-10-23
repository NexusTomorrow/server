using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 18, which carries the action alone; the client raises it as CombatLogImmunity.
/// </summary>
[WireStruct]
[UnionCase(18)]
public sealed partial class CombatLogImmunity : ICombatLogPayload
{
    /// <summary>
    /// The caster, target, spell and combat result.
    /// </summary>
    public CombatLogAction Action { get; set; } = new();
}
