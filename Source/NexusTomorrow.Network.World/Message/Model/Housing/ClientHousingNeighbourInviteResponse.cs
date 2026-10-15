using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ClientHousingNeighborInviteResponse, Direction.ClientToServer)]
public sealed partial class ClientHousingNeighbourInviteResponse
{
    [UInt(1)] public bool Accept { get; private set; }
}
