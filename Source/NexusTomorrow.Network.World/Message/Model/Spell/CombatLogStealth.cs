using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 24: stealth entered or left.
/// </summary>
[WireStruct]
[UnionCase(24)]
public sealed partial class CombatLogStealth : ICombatLogPayload
{
    /// <summary>
    /// The caster, by unit id.
    /// </summary>
    [UInt(32)] public uint CasterId { get; set; }

    /// <summary>
    /// Whether the caster is leaving stealth.
    /// </summary>
    [UInt(1)] public bool Exiting { get; set; }
}
