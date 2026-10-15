using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NetworkMessage = NexusTomorrow.Network.Message.Model.Shared.Message;

namespace NexusTomorrow.Network.Auth.Message.Model;

[WireMessage(GameMessageOpcode.ServerRealmMessages, Direction.ServerToClient)]
public sealed partial class ServerRealmMessages
{
    [Array(CountBits = 32)] public List<NetworkMessage> Messages { get; set; } = [];
}
