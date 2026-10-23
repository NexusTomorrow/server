using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// The payload of the entries that report one amount: arms 0, 14 and 35.
/// </summary>
[WireStruct]
public partial class CombatLogAmount : ICombatLogPayload
{
    /// <summary>
    /// The amount the entry reports.
    /// </summary>
    [UInt(32)] public uint Amount { get; set; }

    /// <summary>
    /// The caster, target, spell and combat result.
    /// </summary>
    public CombatLogAction Action { get; set; } = new();
}
