using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[Message(GameMessageOpcode.ServerCostume)]
public class ServerCostume : IWritable
{
    public Costume Costume { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Costume.Write(writer);
    }
}
