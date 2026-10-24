using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Reports the cinematic camera's subject as a spline.
/// </summary>
[WireMessage(GameMessageOpcode.Client071E, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicCameraSubjectSpline
{
    /// <summary>
    /// The spline, a Spline2 id.
    /// </summary>
    [UInt(32)] public uint SplineId { get; private set; }

    /// <summary>
    /// The mode the spline is evaluated in.
    /// </summary>
    [UInt(32)] public uint SplineMode { get; private set; }

    /// <summary>
    /// A value of the spline's, scaled by <see cref="CinematicCameraSpline.Speed"/>.
    /// </summary>
    [Float32] public float Rate { get; private set; }
}
