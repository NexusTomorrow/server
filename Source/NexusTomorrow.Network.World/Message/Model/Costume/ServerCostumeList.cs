using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[WireMessage(GameMessageOpcode.Server00D9, Direction.ServerToClient)]
public sealed partial class Server00D9
{
    [Array(CountBits = 32)] public List<Server00D8Part0> Unknown0 { get; set; } = [];
}
