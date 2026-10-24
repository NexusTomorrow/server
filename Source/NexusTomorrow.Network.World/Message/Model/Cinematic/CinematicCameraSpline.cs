using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Moves the cinematic camera along a spline, from the command's time.
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

    [UInt(32)] public uint Unknown2 { get; set; }
    [Float32] public float Unknown3 { get; set; }
    [UInt(1)] public bool Unknown4 { get; set; }
    [UInt(1)] public bool Unknown5 { get; set; }
}
