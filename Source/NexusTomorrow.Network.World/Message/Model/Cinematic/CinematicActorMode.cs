using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Changes a cinematic actor's mode at the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0231, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicActorMode
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The actor, by the <see cref="CinematicActorAdd.ActorUnitId"/> that added it.
    /// </summary>
    [UInt(32)] public uint ActorUnitId { get; set; }

    /// <summary>
    /// The actor's new mode, the one <see cref="CinematicActorAdd.Mode"/> first sets.
    /// </summary>
    [UInt(32)] public uint Mode { get; set; }
}
