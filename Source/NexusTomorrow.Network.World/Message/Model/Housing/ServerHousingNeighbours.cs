using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingNeighbors, Direction.ServerToClient)]
public sealed partial class ServerHousingNeighbours
{
    [Array(CountBits = 32)] public List<Neighbour> Neighbours { get; set; } = [];
}
