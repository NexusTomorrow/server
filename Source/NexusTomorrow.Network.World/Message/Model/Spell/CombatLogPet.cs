using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 26: a pet dismissed or killed.
/// </summary>
[WireStruct]
[UnionCase(26)]
public sealed partial class CombatLogPet : ICombatLogPayload
{
    /// <summary>
    /// Whether the pet was dismissed.
    /// </summary>
    [UInt(1)] public bool Dismissed { get; set; }

    /// <summary>
    /// Whether the pet was killed.
    /// </summary>
    [UInt(1)] public bool Killed { get; set; }

    /// <summary>
    /// The caster, target, spell and combat result.
    /// </summary>
    public CombatLogAction Action { get; set; } = new();
}
