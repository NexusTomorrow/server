using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server0507, Direction.ServerToClient)]
public sealed partial class Server0507
{
    [Array(CountBits = 32)] public List<Server0507Part0> Unknown0 { get; set; } = [];
}
