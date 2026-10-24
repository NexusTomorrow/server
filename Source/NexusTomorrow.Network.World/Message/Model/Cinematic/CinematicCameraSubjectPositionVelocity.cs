using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Reports the cinematic camera's subject as a position and a velocity.
/// </summary>
[WireMessage(GameMessageOpcode.Client071D, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicCameraSubjectPositionVelocity
{
    /// <summary>
    /// The subject's position.
    /// </summary>
    public Client0096Part0 Position { get; private set; } = new();

    /// <summary>
    /// The subject's velocity.
    /// </summary>
    [Float32] public Vector3 Velocity { get; private set; }
}
