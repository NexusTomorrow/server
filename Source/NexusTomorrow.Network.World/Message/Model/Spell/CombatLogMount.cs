using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 25: a mount taken or left.
/// </summary>
[WireStruct]
[UnionCase(25)]
public sealed partial class CombatLogMount : ICombatLogPayload
{
    /// <summary>
    /// Whether the unit dismounted.
    /// </summary>
    [UInt(1)] public bool Dismounted { get; set; }

    /// <summary>
    /// The caster, target, spell and combat result.
    /// </summary>
    public CombatLogAction Action { get; set; } = new();
}
