using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

[WireMessage(GameMessageOpcode.Server0981, Direction.ServerToClient)]
public sealed partial class Server0981
{
    [Array(CountBits = 32), UInt(32)] public List<uint> Unknown0 { get; set; } = [];
}
