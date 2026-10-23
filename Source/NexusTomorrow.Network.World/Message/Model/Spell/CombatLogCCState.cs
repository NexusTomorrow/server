using NexusTomorrow.Game.Static.Combat.CrowdControl;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 1: a CC state applied to the target or removed from it.
/// </summary>
[WireStruct]
[UnionCase(1)]
public sealed partial class CombatLogCCState : ICombatLogPayload
{
    /// <summary>
    /// The CC state.
    /// </summary>
    [UInt(5)] public CCState State { get; set; }

    /// <summary>
    /// Whether the state was removed rather than applied.
    /// </summary>
    [UInt(1)] public bool Removed { get; set; }

    /// <summary>
    /// How much interrupt armor the hit took.
    /// </summary>
    [UInt(32)] public uint InterruptArmorHit { get; set; }

    /// <summary>
    /// The result of applying the state.
    /// </summary>
    [UInt(4)] public byte Result { get; set; }

    /// <summary>
    /// The state's trigger cap category.
    /// </summary>
    [UInt(14)] public ushort TriggerCapCategory { get; set; }

    /// <summary>
    /// The caster, target, spell and combat result.
    /// </summary>
    public CombatLogAction Action { get; set; } = new();
}
