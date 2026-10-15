using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicCameraPosVel)]
[WireMessage(GameMessageOpcode.ServerCinematicCameraPosVel, Direction.ServerToClient)]
public sealed partial class ServerCinematicCameraPosVel : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    public Position Position { get; set; }
    [Float32] public Vector3 Velocity { get; set; }
    [UInt(1)] public bool Target { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        Position.Write(writer);
        writer.Write(Velocity.X);
        writer.Write(Velocity.Y);
        writer.Write(Velocity.Z);
        writer.Write(Target);
    }
}

