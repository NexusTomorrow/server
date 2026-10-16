using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server0975, Direction.ServerToClient)]
public sealed partial class Server0975
{
    public Server096APart0 Unknown0 { get; set; } = new();
}
