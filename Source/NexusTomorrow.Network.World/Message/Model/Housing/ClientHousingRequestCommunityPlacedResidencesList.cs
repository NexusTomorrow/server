using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ClientHousingRequestCommunityPlacedResidencesList, Direction.ClientToServer)]
public sealed partial class ClientHousingRequestCommunityPlacedResidencesList
{
}
