using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Client0956, Direction.ClientToServer)]
public sealed partial class Client0956
{
    public Client00B5Part0 Unknown0 { get; private set; } = new();
}
