using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Client052F, Direction.ClientToServer)]
public sealed partial class Client052F
{
    public Client00B5Part0 Unknown0 { get; } = new();
}
