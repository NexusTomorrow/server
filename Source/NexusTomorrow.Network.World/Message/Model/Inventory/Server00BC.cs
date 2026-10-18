using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

[WireMessage(GameMessageOpcode.Server00BC, Direction.ServerToClient)]
public sealed partial class Server00BC
{
    [Array(CountBits = 32)] public List<Server00BAPart0> Unknown0 { get; } = [];
}
