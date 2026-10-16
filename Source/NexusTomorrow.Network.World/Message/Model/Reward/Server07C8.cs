using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Server07C8, Direction.ServerToClient)]
public sealed partial class Server07C8
{
    [Array(CountBits = 32)] public List<Server07C7Part0> Unknown0 { get; set; } = [];
}
