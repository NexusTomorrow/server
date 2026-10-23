using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 30: an item destroyed.
/// </summary>
/// <remarks>
/// The client reports it with the loot updates, as an item destroyed.
/// </remarks>
[WireStruct]
[UnionCase(30)]
public sealed partial class CombatLogItemDestroyed : ICombatLogPayload
{
    /// <summary>
    /// The caster, by unit id.
    /// </summary>
    [UInt(32)] public uint CasterId { get; set; }

    /// <summary>
    /// The item, an Item2 table id.
    /// </summary>
    [UInt(18)] public uint ItemId { get; set; }
}
