using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client03C7, Direction.ClientToServer)]
public sealed partial class Client03C7
{
    public Client00B5Part0 Unknown0 { get; private set; } = new();
    [WideString] public string Unknown1 { get; private set; } = "";
}
