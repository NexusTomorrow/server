using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Who.Parameter;

namespace NexusTomorrow.Network.World.Message.Model.Who;

// If no parameters are sent, send a who response for the current zone with sensible parameters
// Can get sent multiple sets of parameters that AND and OR together
[WireMessage(GameMessageOpcode.ClientWhoRequest, Direction.ClientToServer)]
public sealed partial class ClientWhoRequest
{
    [Array(CountBits = 32)] public List<WhoParameter> Parameters { get; private set; } = [];
    [Array(CountBits = 32), UInt(32)] public List<uint> ParameterGroupCounts { get; private set; } = []; // Number of ANDed parameters in each parameter group
}
