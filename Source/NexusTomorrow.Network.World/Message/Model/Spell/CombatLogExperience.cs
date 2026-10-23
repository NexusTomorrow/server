using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 27: experience gained.
/// </summary>
[WireStruct]
[UnionCase(27)]
public sealed partial class CombatLogExperience : ICombatLogPayload
{
    /// <summary>
    /// The caster, by unit id.
    /// </summary>
    [UInt(32)] public uint CasterId { get; set; }

    /// <summary>
    /// The experience gained.
    /// </summary>
    [UInt(32)] public uint Xp { get; set; }

    /// <summary>
    /// The rested experience gained.
    /// </summary>
    [UInt(32)] public uint RestXp { get; set; }

    /// <summary>
    /// The elder points gained.
    /// </summary>
    [UInt(32)] public uint ElderPoints { get; set; }

    /// <summary>
    /// The rested elder points gained.
    /// </summary>
    [UInt(32)] public uint RestElderPoints { get; set; }
}
