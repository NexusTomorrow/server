using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// The payload of the entries that name only a unit, such as a death, a resurrection or a change of action set: arms 22, 23 and 31.
/// </summary>
[WireStruct]
public partial class CombatLogUnit : ICombatLogPayload
{
    /// <summary>
    /// The caster, by unit id.
    /// </summary>
    [UInt(32)] public uint CasterId { get; set; }
}
