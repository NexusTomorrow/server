using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientMatchingMatchLeave)]
public class ClientMatchingMatchLeave : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // deliberately empty
    }
}
