using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 20: a kill streak.
/// </summary>
[WireStruct]
[UnionCase(20)]
public sealed partial class CombatLogKillStreak : ICombatLogPayload
{
    /// <summary>
    /// The caster, by unit id.
    /// </summary>
    [UInt(32)] public uint CasterId { get; set; }

    /// <summary>
    /// The kind of streak.
    /// </summary>
    [UInt(5)] public byte StatType { get; set; }

    /// <summary>
    /// The streak's count.
    /// </summary>
    [UInt(32)] public uint StreakAmount { get; set; }
}
