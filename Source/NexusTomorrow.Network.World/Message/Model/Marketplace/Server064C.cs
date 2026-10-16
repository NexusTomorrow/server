using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server064C, Direction.ServerToClient)]
public sealed partial class Server064C
{
    [Array(CountBits = 32)] public List<Server024APart0> Unknown0 { get; set; } = [];
}
