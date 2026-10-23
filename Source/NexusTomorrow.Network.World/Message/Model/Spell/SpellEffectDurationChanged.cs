using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Changes how long one of a cast's effects lasts on a target.
/// </summary>
[WireMessage(GameMessageOpcode.Server0812, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SpellEffectDurationChanged
{
    /// <summary>
    /// The cast, by the server's id for it.
    /// </summary>
    [UInt(32)] public uint CastId { get; set; }

    /// <summary>
    /// The effect, by id.
    /// </summary>
    [UInt(32)] public uint EffectId { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }

    /// <summary>
    /// The target, by unit id.
    /// </summary>
    [UInt(32)] public uint TargetUnitId { get; set; }
}
