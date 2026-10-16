using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Client06DA, Direction.ClientToServer)]
public sealed partial class Client06DA
{
    public Server024APart0 Unknown0 { get; private set; } = new();
}
