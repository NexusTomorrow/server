using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Ends the white-out <see cref="CinematicWhiteOut"/> began. After the white-out's time it fades the screen back in; if the client
/// answered the white-out at once because the camera was holding, it instead releases a continue <see cref="CinematicContinue"/> deferred.
/// </summary>
[WireMessage(GameMessageOpcode.Server022C, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicWhiteOutEnd
{
    [UInt(1)] public bool Unknown0 { get; set; }

    /// <summary>
    /// The camera's yaw, in radians; the client wraps it to [-pi, pi).
    /// </summary>
    [Float32] public float Yaw { get; set; }

    [Float32] public float Unknown2 { get; set; }
}
