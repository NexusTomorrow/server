using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

[WireMessage(GameMessageOpcode.ServerHazardModifiers, Direction.ServerToClient)]
public sealed partial class ServerHazardModifiers
{
    [Array(CountBits = 32)] public List<HazardModifier> HazardIdModifiers { get; set; } = [];
    [Array(CountBits = 32)] public List<HazardModifier> HazardTypeModifiers { get; set; } = [];
}
