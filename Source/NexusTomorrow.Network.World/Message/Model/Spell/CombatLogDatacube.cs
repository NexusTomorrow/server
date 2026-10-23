using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 33: a datacube.
/// </summary>
[WireStruct]
[UnionCase(33)]
public sealed partial class CombatLogDatacube : ICombatLogPayload
{
    /// <summary>
    /// The caster, by unit id.
    /// </summary>
    [UInt(32)] public uint CasterId { get; set; }

    /// <summary>
    /// The datacube's type.
    /// </summary>
    [UInt(3)] public byte DatacubeType { get; set; }

    /// <summary>
    /// Whether the datacube comes in pieces.
    /// </summary>
    [UInt(1)] public bool HasPieces { get; set; }
}
