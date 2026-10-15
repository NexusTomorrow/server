using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ClientCinematicCameraSubjectPosVel, Direction.ClientToServer)]
public sealed partial class ClientCinematicCameraSubjectPosVel
{
    public Position Position { get; private set; }
    [Float32] public Vector3 Velocity { get; private set; }
}
