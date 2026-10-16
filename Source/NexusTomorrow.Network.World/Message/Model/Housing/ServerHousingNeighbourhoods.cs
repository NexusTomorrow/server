using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server0506, Direction.ServerToClient)]
public sealed partial class Server0506
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Server0501Part0> Unknown1 { get; set; } = [];
}
