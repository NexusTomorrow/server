using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 15: a transference, which deals damage and heals.
/// </summary>
[WireStruct]
[UnionCase(15)]
public sealed partial class CombatLogTransference : ICombatLogPayload
{
    /// <summary>
    /// The damage dealt.
    /// </summary>
    [UInt(32)] public uint DamageAmount { get; set; }

    /// <summary>
    /// The damage type.
    /// </summary>
    [UInt(3)] public byte DamageType { get; set; }

    /// <summary>
    /// The damage the target's shields took.
    /// </summary>
    [UInt(32)] public uint Shield { get; set; }

    /// <summary>
    /// The damage absorbed.
    /// </summary>
    [UInt(32)] public uint Absorption { get; set; }

    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(32)] public uint Unknown5 { get; set; }
    [UInt(1)] public bool Unknown6 { get; set; }

    /// <summary>
    /// The units the transference healed.
    /// </summary>
    [Array(CountBits = 32)] public List<CombatLogTransferenceHeal> Heals { get; set; } = [];

    /// <summary>
    /// The caster, target, spell and combat result.
    /// </summary>
    public CombatLogAction Action { get; set; } = new();
}
