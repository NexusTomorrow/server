using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

[WireMessage(GameMessageOpcode.Message010A, Direction.ServerToClient)]
public sealed partial class HazardStates
{
    [Array(CountBits = 32)] public List<HazardState> Unknown0 { get; set; } = [];
}
