using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server0966, Direction.ServerToClient)]
public sealed partial class Server0966
{
    [Array(CountBits = 32)] public List<Server0966Part0> Unknown0 { get; set; } = [];
}
