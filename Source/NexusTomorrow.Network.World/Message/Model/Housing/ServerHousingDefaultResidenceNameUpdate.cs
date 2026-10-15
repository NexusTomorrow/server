using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingDefaultResidenceNameUpdate, Direction.ServerToClient)]
public sealed partial class ServerHousingDefaultResidenceNameUpdate : Neighbourhood
{
    // Client only uses the Name of the Neighbourhood in this message
}
