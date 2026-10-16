using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server0564, Direction.ServerToClient)]
public sealed partial class Server0564
{
    public Server055FPart0 Unknown0 { get; set; } = new();
}
