using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerEntityDestroy)]
public class ServerEntityDestroy : IWritable
{
    public uint Guid { get; set; }
    public bool Unknown0 { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Guid);
        writer.Write(Unknown0);
    }
}
