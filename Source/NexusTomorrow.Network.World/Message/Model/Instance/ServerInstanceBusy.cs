using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

// Prevents resurrection interaction and causes InstanceSetBusy event
[Message(GameMessageOpcode.ServerInstanceBusy)]
public class ServerInstanceBusy : IWritable
{
    public bool Busy { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Busy);
    }
}
