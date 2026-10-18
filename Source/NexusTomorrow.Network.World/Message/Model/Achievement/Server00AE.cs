using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Achievement;

[WireMessage(GameMessageOpcode.Server00AE, Direction.ServerToClient)]
public sealed partial class Server00AE
{
    [Array(CountBits = 32)] public List<Server00AEPart0> Unknown0 { get; set; } = [];
}
