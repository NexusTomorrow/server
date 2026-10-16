using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server0767, Direction.ServerToClient)]
public sealed partial class Server0767
{
    [Array(CountBits = 32)] public List<Server0767Part0> Unknown0 { get; set; }
}
