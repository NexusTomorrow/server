using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Server0111, Direction.ServerToClient)]
public sealed partial class Server0111
{
    public Server0111Part0 Unknown0 { get; set; } = new();
}
