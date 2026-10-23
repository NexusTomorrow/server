using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Runs one of a cast's effects on a target.
/// </summary>
[WireMessage(GameMessageOpcode.Server07F5, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SpellEffectExecute
{
    /// <summary>
    /// The cast, by the server's id for it.
    /// </summary>
    [UInt(32)] public uint CastId { get; set; }

    /// <summary>
    /// The target, by unit id.
    /// </summary>
    [UInt(32)] public uint TargetUnitId { get; set; }

    /// <summary>
    /// The effect, by id.
    /// </summary>
    [UInt(32)] public uint EffectId { get; set; }
}
