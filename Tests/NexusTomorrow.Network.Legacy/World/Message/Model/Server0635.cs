using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.Server0635)]
public class Server0635 : IWritable
{
    public uint Unknown0 { get; set; } = 0;

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Unknown0);
    }
}
