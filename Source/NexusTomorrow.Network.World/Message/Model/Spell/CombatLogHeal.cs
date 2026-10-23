using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// The payload of the heal entries: arms 12 and 13.
/// </summary>
[WireStruct]
public partial class CombatLogHeal : ICombatLogPayload
{
    /// <summary>
    /// The amount healed.
    /// </summary>
    [UInt(32)] public uint HealAmount { get; set; }

    /// <summary>
    /// The healing beyond what the target was missing.
    /// </summary>
    [UInt(32)] public uint Overheal { get; set; }

    /// <summary>
    /// The healing absorbed; the client reads it for kind 13 alone.
    /// </summary>
    [UInt(32)] public uint Absorption { get; set; }

    /// <summary>
    /// The type of the spell effect that healed.
    /// </summary>
    [UInt(8)] public byte EffectType { get; set; }

    /// <summary>
    /// The caster, target, spell and combat result.
    /// </summary>
    public CombatLogAction Action { get; set; } = new();
}
