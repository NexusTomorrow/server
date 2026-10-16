using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Crafting;

[WireMessage(GameMessageOpcode.Server0860, Direction.ServerToClient)]
public sealed partial class Server0860
{
    public Server0856Part0 Unknown0 { get; set; } = new Server0856Part0();
}
