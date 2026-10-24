using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Has the client build an entity command for a cinematic actor at the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0230, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicActorFacing
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

    [Float32] public float Unknown2 { get; set; }
}
