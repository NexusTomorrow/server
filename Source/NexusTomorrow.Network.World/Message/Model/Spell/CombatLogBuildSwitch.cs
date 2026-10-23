using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 32: a build switched.
/// </summary>
[WireStruct]
[UnionCase(32)]
public sealed partial class CombatLogBuildSwitch : ICombatLogPayload
{
    /// <summary>
    /// The caster, by unit id.
    /// </summary>
    [UInt(32)] public uint CasterId { get; set; }

    /// <summary>
    /// The build switched to, by index.
    /// </summary>
    [UInt(3)] public byte NewSpecIndex { get; set; }
}
