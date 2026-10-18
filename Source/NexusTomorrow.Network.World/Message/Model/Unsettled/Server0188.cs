using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server0188, Direction.ServerToClient)]
public sealed partial class Server0188
{
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown0 { get; } = [];
}
