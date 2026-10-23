using NexusTomorrow.Game.Static.Combat.CrowdControl;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 2: a CC state broken.
/// </summary>
[WireStruct]
[UnionCase(2)]
public sealed partial class CombatLogCCStateBreak : ICombatLogPayload
{
    /// <summary>
    /// The caster, by unit id.
    /// </summary>
    [UInt(32)] public uint CasterId { get; set; }

    /// <summary>
    /// The CC state.
    /// </summary>
    [UInt(5)] public CCState State { get; set; }
}
