using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerGameCameraRotation)]
public class ServerGameCameraRotation : IWritable
{
    public float Yaw { get; set; } // in radians
    public float Pitch { get; set; } // in radians

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Yaw);
        writer.Write(Pitch);
    }
}
