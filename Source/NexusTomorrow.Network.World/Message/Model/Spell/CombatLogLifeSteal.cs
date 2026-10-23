using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 9: health stolen.
/// </summary>
[WireStruct]
[UnionCase(9)]
public sealed partial class CombatLogLifeSteal : ICombatLogPayload
{
    /// <summary>
    /// The caster, by unit id.
    /// </summary>
    [UInt(32)] public uint CasterId { get; set; }

    /// <summary>
    /// The health stolen.
    /// </summary>
    [UInt(32)] public uint HealthStolen { get; set; }

    /// <summary>
    /// The amount absorbed.
    /// </summary>
    [UInt(32)] public uint Absorption { get; set; }
}
