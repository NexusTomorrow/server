using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Who acted on whom, with which spell, and how it landed: the record most combat log entries end with.
/// </summary>
[WireStruct]
public sealed partial class CombatLogAction
{
    /// <summary>
    /// The caster, by unit id.
    /// </summary>
    [UInt(32)] public uint CasterId { get; set; }

    /// <summary>
    /// The target, by unit id.
    /// </summary>
    [UInt(32)] public uint TargetId { get; set; }

    /// <summary>
    /// The spell, a Spell4 table id.
    /// </summary>
    [UInt(18)] public uint SpellId { get; set; }

    /// <summary>
    /// How the action landed, as a combat result.
    /// </summary>
    [UInt(4)] public byte CombatResult { get; set; }
}
