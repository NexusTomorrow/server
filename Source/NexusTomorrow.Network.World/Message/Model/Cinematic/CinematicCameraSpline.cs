using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Moves the cinematic camera's position, or the point it looks at, along a spline, from the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0215, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicCameraSpline
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The spline, a Spline2 id.
    /// </summary>
    [UInt(32)] public uint SplineId { get; set; }

    /// <summary>
    /// The mode the client evaluates the spline in.
    /// </summary>
    [UInt(32)] public uint SplineMode { get; set; }

    /// <summary>
    /// Scales how fast the camera's channel moves along the spline.
    /// </summary>
    [Float32] public float Speed { get; set; }

    /// <summary>
    /// Sets the camera's look-at channel instead of its position channel.
    /// </summary>
    [UInt(1)] public bool LookAt { get; set; }

    /// <summary>
    /// On the position channel, has the client report this spline as the camera's subject in
    /// <see cref="CinematicCameraSubjectSpline"/>.
    /// </summary>
    [UInt(1)] public bool ReportSpline { get; set; }
}
