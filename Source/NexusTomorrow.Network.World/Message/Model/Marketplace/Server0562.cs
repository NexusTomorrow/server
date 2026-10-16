using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server0562, Direction.ServerToClient)]
public sealed partial class Server0562
{
    public Server055FPart0 Unknown0 { get; set; }
}
