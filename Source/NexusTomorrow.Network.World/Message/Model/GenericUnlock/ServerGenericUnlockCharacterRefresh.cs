using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

[WireMessage(GameMessageOpcode.Server0984, Direction.ServerToClient)]
public sealed partial class Server0984
{
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown0 { get; set; } = [];
}
