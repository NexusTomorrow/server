using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server0979, Direction.ServerToClient)]
public sealed partial class Server0979
{
    [Array(CountBits = 32)] public List<Server0976Part0> Unknown0 { get; set; } = [];
}
