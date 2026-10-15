using System.Numerics;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Entity;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicCameraPosVel)]
public class ServerCinematicCameraPosVel : IWritable
{
    public uint Delay { get; set; }
    public Position Position { get; set; }
    public Vector3 Velocity { get; set; }
    public bool Target { get; set; }

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

