using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Removes a cinematic actor at the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server021D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CinematicActorRemove
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
}
