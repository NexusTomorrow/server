using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[Message(GameMessageOpcode.ClientHousingRequestCommunityPlacedResidencesList)]
[WireMessage(GameMessageOpcode.ClientHousingRequestCommunityPlacedResidencesList, Direction.ClientToServer)]
public sealed partial class ClientHousingRequestCommunityPlacedResidencesList : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // Zero byte message
    }
}
