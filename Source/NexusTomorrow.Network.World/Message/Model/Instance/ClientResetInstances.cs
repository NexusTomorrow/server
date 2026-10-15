using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[Message(GameMessageOpcode.ClientResetInstances)]
public class ClientResetInstances : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // Zero byte message
    }
}
