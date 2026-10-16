using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client05D1, Direction.ClientToServer)]
public sealed partial class Client05D1
{
    public Client00B5Part0 Unknown0 { get; private set; } = new();
}
