using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientMatchingMatchInitiateVoteToSurrender)]
public class ClientMatchingMatchInitiateVoteToSurrender : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // Zero size message
    }
}
