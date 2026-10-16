using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server064D, Direction.ServerToClient)]
public sealed partial class Server064D
{
    [Array(CountBits = 32)] public List<Server055FPart0> Unknown0 { get; set; } = [];
}
