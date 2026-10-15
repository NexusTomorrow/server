using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingNeighbourInviteReceived, Direction.ServerToClient)]
public sealed partial class ServerHousingNeighbourInviteReceived
{
    public Identity Invitor { get; set; } // Not sure what is being identified here, not used by client
    [WideString] public string InvitorName { get; set; }
}
