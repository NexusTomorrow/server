using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.Message.Model.Shared;

namespace NexusTomorrow.Network.Auth.Message.Model;

[WireMessage(GameMessageOpcode.Server0763, Direction.ServerToClient)]
public sealed partial class Server0763
{
    [Array(CountBits = 32)] public List<Message0761Part2> Unknown0 { get; set; } = [];
}
