using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Moves a cinematic actor along a spline from the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server022F, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicActorSpline
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
    /// The spline, by spline id.
    /// </summary>
    [UInt(32)] public uint SplineId { get; set; }

    /// <summary>
    /// How fast the actor moves along the spline.
    /// </summary>
    [Float32] public float Speed { get; set; }

    [UInt(32)] public uint Unknown4 { get; set; }
    [UInt(1)] public bool Unknown5 { get; set; }
    [UInt(1)] public bool Unknown6 { get; set; }
}
