using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGameCameraRotation)]
[WireMessage(GameMessageOpcode.ServerGameCameraRotation, Direction.ServerToClient)]
public sealed partial class ServerGameCameraRotation : IWritable
{
    [Float32] public float Yaw { get; set; } // in radians
    [Float32] public float Pitch { get; set; } // in radians

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Yaw);
        writer.Write(Pitch);
    }
}
