using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

[Message(GameMessageOpcode.ServerCinematicCameraRotation)]
[WireMessage(GameMessageOpcode.ServerCinematicCameraRotation, Direction.ServerToClient)]
public sealed partial class ServerCinematicCameraRotation : IWritable
{
    [UInt(32)] public uint Delay { get; set; }
    [Float32] public float Yaw { get; set; } // in radians, multiple rotations are normalized to [0, 2pi]
    [Float32] public float Pitch { get; set; } // in radians, limited to [-pi/2, pi/2]

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Delay);
        writer.Write(Yaw);
        writer.Write(Pitch);
    }
}
