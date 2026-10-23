using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// One unit a transference healed.
/// </summary>
[WireStruct]
public sealed partial class CombatLogTransferenceHeal
{
    /// <summary>
    /// The unit healed, by unit id.
    /// </summary>
    [UInt(32)] public uint HealedUnitId { get; set; }

    /// <summary>
    /// The amount healed.
    /// </summary>
    [UInt(32)] public uint HealAmount { get; set; }

    /// <summary>
    /// The vital healed.
    /// </summary>
    [UInt(5)] public byte VitalType { get; set; }

    /// <summary>
    /// The healing beyond what the unit was missing.
    /// </summary>
    [UInt(32)] public uint Overheal { get; set; }

    /// <summary>
    /// The healing absorbed.
    /// </summary>
    [UInt(32)] public uint Absorption { get; set; }
}
