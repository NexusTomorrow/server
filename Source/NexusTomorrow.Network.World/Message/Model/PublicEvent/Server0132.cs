using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.PublicEvent;

[WireMessage(GameMessageOpcode.Server0132, Direction.ServerToClient)]
public sealed partial class Server0132
{
    public Server0112Part0 Unknown0 { get; set; } = new();
}
