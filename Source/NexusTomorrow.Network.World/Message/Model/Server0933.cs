using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0933, Direction.ServerToClient)]
public sealed partial class Server0933
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Server0117Part1> Unknown1 { get; set; } = [];
}
