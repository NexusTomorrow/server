using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 11: an effect dispelled.
/// </summary>
[WireStruct]
[UnionCase(11)]
public sealed partial class CombatLogDispel : ICombatLogPayload
{
    /// <summary>
    /// Whether the dispel removes a single instance.
    /// </summary>
    [UInt(1)] public bool RemovesSingleInstance { get; set; }

    /// <summary>
    /// How many instances the dispel removed.
    /// </summary>
    [UInt(32)] public uint InstancesRemoved { get; set; }

    /// <summary>
    /// The spell removed, a Spell4 table id.
    /// </summary>
    [UInt(18)] public uint RemovedSpellId { get; set; }

    /// <summary>
    /// The caster, target, spell and combat result.
    /// </summary>
    public CombatLogAction Action { get; set; } = new();
}
