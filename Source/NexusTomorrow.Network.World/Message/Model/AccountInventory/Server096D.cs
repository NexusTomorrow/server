using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.Server096D, Direction.ServerToClient)]
public sealed partial class Server096D
{
    [Array(CountBits = 32)] public List<Server096APart0> Unknown0 { get; set; } = [];
}
