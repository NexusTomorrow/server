using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[WireMessage(GameMessageOpcode.ServerCinematicCameraPosVel, Direction.ServerToClient)]
public sealed partial class ServerCinematicCameraPosVel
{
    [UInt(32)] public uint Delay { get; set; }
    public Position Position { get; set; }
    [Float32] public Vector3 Velocity { get; set; }
    [UInt(1)] public bool Target { get; set; }
}

