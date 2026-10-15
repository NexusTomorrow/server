using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Cinematic;

// Starts the outro transition of the cinematic, returning to the game camera
[Message(GameMessageOpcode.ServerCinematicEnd)]
[WireMessage(GameMessageOpcode.ServerCinematicEnd, Direction.ServerToClient)]
public sealed partial class ServerCinematicEnd : IWritable
{
    [UInt(1)] public bool SetGameCamera { get; set; }
    [Float32] public float Yaw { get; set; } // in radians, multiple rotations are normalized to [0, 2pi]
    [Float32] public float Pitch { get; set; } // in radians, limited to [-pi/2, pi/2]

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SetGameCamera);
        writer.Write(Yaw);
        writer.Write(Pitch);
    }
}
