using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerLogout)]
public class ServerLogout : IWritable
{
    public bool Requested { get; set; }
    public LogoutReason Reason { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Requested);
        writer.Write(Reason, 5);
    }
}
