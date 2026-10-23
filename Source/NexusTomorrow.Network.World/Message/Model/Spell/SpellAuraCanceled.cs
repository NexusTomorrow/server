using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Cancels a cast's aura on a target.
/// </summary>
[WireMessage(GameMessageOpcode.Server0819, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SpellAuraCanceled
{
    /// <summary>
    /// The target, by unit id.
    /// </summary>
    [UInt(32)] public uint TargetUnitId { get; set; }

    /// <summary>
    /// The cast, by the server's id for it.
    /// </summary>
    [UInt(32)] public uint CastId { get; set; }
}
