using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.ServerHousingNeighborhoods, Direction.ServerToClient)]
public sealed partial class ServerHousingNeighbourhoods
{
    [UInt(14)] public ushort Unused { get; set; }
    [Array(CountBits = 32)] public List<Neighbourhood> Neighbourhoods { get; set; } = [];
}
