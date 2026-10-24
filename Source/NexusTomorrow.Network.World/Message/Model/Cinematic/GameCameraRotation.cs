using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Sets the camera's rotation at once, outside a cinematic's command queue.
/// </summary>
[WireMessage(GameMessageOpcode.Server0091, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GameCameraRotation
{
    /// <summary>
    /// The camera's yaw, in radians; the client wraps it to [-pi, pi).
    /// </summary>
    [Float32] public float Yaw { get; set; }

    [Float32] public float Unknown1 { get; set; }
}
