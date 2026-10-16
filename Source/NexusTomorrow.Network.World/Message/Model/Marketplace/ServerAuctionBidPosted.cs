using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server055F, Direction.ServerToClient)]
public sealed partial class Server055F
{
    public Server055FPart0 Unknown0 { get; set; }
}
