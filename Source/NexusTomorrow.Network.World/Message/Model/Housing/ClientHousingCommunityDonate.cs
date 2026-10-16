using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client04F5, Direction.ClientToServer)]
public sealed partial class Client04F5
{
    [Array(CountBits = 32)] public List<Client04F5Part0> Unknown0 { get; private set; } = [];
}
