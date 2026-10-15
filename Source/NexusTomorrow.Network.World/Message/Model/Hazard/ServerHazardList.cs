using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

[WireMessage(GameMessageOpcode.ServerHazardList, Direction.ServerToClient)]
public sealed partial class ServerHazardList
{
    [Array(CountBits = 32)] public List<Hazard> Hazards { get; set; } = [];
}
