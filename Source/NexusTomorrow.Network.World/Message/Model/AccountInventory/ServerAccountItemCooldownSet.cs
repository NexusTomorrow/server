using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server0974, Direction.ServerToClient)]
public sealed partial class Server0974
{
    public Server0969Part0 Unknown0 { get; set; }
}
