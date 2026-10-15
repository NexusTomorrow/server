using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingRequestCommunityPlacedResidencesList)]
public class ClientHousingRequestCommunityPlacedResidencesList : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // Zero byte message
    }
}
