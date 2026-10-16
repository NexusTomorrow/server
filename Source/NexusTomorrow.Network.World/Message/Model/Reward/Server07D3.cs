using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Server07D3, Direction.ServerToClient)]
public sealed partial class Server07D3
{
    [Array(CountBits = 32)] public List<Server07CDPart0> Unknown0 { get; set; } = [];
}
