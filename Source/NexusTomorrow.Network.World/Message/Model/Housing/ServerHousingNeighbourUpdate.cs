using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingNeighbourUpdate, Direction.ServerToClient)]
public sealed partial class ServerHousingNeighbourUpdate
{
    public Neighbour Neighbour { get; set; }
    [UInt(3)] public NeighborResult Result { get; set; }
}
