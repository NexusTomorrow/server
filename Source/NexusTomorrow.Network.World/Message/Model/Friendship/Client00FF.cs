using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client00FF, Direction.ClientToServer)]
public sealed partial class Client00FF
{
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown0 { get; } = [];
}
