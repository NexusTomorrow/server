using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Takes one stack off a cast's target.
/// </summary>
[WireMessage(GameMessageOpcode.Server0813, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SpellTargetStackRemoved
{
    /// <summary>
    /// The cast, by the server's id for it.
    /// </summary>
    [UInt(32)] public uint CastId { get; set; }

    /// <summary>
    /// The target, by unit id.
    /// </summary>
    [UInt(32)] public uint TargetUnitId { get; set; }
}
