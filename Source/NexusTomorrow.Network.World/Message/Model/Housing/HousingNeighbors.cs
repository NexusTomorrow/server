using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server0507, Direction.ServerToClient)]
public sealed partial class HousingNeighbors
{
    [Array(CountBits = 32)] public List<HousingNeighbor> Unknown0 { get; set; } = [];
}
