using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 28: an item modified.
/// </summary>
[WireStruct]
[UnionCase(28)]
public sealed partial class CombatLogModifying : ICombatLogPayload
{
    /// <summary>
    /// The caster, by unit id.
    /// </summary>
    [UInt(32)] public uint CasterId { get; set; }

    /// <summary>
    /// The item modified.
    /// </summary>
    [UInt(18)] public uint HostItemId { get; set; }

    /// <summary>
    /// The item added to it.
    /// </summary>
    [UInt(32)] public uint AddedItemId { get; set; }

    /// <summary>
    /// The item removed from it.
    /// </summary>
    [UInt(32)] public uint RemovedItemId { get; set; }
}
