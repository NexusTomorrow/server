using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 19: a cast interrupted.
/// </summary>
[WireStruct]
[UnionCase(19)]
public sealed partial class CombatLogInterrupted : ICombatLogPayload
{
    /// <summary>
    /// The spell that interrupted the cast, a Spell4 table id.
    /// </summary>
    [UInt(18)] public uint InterruptingSpellId { get; set; }

    /// <summary>
    /// Why the cast stopped, a Spell4CastResult table id.
    /// </summary>
    [UInt(9)] public ushort CastResult { get; set; }

    /// <summary>
    /// The caster, target, spell and combat result.
    /// </summary>
    public CombatLogAction Action { get; set; } = new();
}
