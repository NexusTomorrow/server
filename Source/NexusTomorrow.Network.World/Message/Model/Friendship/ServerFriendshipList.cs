using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Server03BE, Direction.ServerToClient)]
public sealed partial class Server03BE
{
    [Array(CountBits = 16)] public List<Server03B5Part0> Unknown0 { get; set; } = [];
}
