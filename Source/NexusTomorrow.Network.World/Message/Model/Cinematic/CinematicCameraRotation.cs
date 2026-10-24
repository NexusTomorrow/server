using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Sets the camera's rotation at the command's time.
/// </summary>
[WireMessage(GameMessageOpcode.Server0221, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicCameraRotation
{
    /// <summary>
    /// When the client runs the command, in milliseconds after the cinematic's start or after the last
    /// <see cref="CinematicTimeRebase"/> it ran.
    /// </summary>
    [UInt(32)] public uint TimeMs { get; set; }

    /// <summary>
    /// The camera's yaw, in radians; the client wraps it to [-pi, pi).
    /// </summary>
    [Float32] public float Yaw { get; set; }

    /// <summary>
    /// The camera's pitch, in radians; the client clamps it to +-pi/2.
    /// </summary>
    [Float32] public float Pitch { get; set; }
}
