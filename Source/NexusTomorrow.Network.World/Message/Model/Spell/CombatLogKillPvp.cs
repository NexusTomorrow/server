using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 21: a kill in PvP.
/// </summary>
[WireStruct]
[UnionCase(21)]
public sealed partial class CombatLogKillPvp : ICombatLogPayload
{
    /// <summary>
    /// The caster, target, spell and combat result.
    /// </summary>
    public CombatLogAction Action { get; set; } = new();
}
