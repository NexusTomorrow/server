using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

// Not seen in sniffs but there is lua UI code for handling the triggered event
[Message(GameMessageOpcode.ServerShowInstanceRestrictedDialog)]
public class ServerShowInstanceRestrictedDialog : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // Zero byte message
    }
}
