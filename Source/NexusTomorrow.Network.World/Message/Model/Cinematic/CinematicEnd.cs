using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Ends the white-out <see cref="CinematicWhiteOut"/> began, first setting the camera's rotation when asked.
/// </summary>
[WireMessage(GameMessageOpcode.Server022C, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicEnd
{
    /// <summary>
    /// Whether the client applies <see cref="Yaw"/> and <see cref="Pitch"/> to the camera.
    /// </summary>
    [UInt(1)] public bool ApplyRotation { get; set; }

    /// <summary>
    /// The camera's yaw, in radians; the client wraps it to [-pi, pi).
    /// </summary>
    [Float32] public float Yaw { get; set; }

    /// <summary>
    /// The camera's pitch, in radians; the client clamps it to +-pi/2.
    /// </summary>
    [Float32] public float Pitch { get; set; }
}
