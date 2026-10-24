using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

/// <summary>
/// Reports the cinematic camera's subject.
/// </summary>
[WireMessage(GameMessageOpcode.Client071D, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class CinematicCameraSubjectPositionVelocity
{
    public Client0096Part0 Unknown0 { get; private set; } = new();
    [Float32] public Vector3 Unknown1 { get; private set; }
}
