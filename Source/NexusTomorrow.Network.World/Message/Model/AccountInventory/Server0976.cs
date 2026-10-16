using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server0976, Direction.ServerToClient)]
public sealed partial class Server0976
{
    public Server0976Part0 Unknown0 { get; set; } = new();
}
