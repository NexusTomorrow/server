using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NetworkMessage = NexusTomorrow.Network.Message.Model.Shared.Message0761Part2;

namespace NexusTomorrow.Network.Auth.Message.Model;

[WireMessage(GameMessageOpcode.Server0763, Direction.ServerToClient)]
public sealed partial class Server0763
{
    [Array(CountBits = 32)] public List<NetworkMessage> Unknown0 { get; set; } = [];
}
