using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

[WireMessage(GameMessageOpcode.Server066E, Direction.ServerToClient)]
public sealed partial class Server066E
{
    [UInt(14)] public ushort Unknown0 { get; set; }
    [Array(CountBits = 32)] public List<Server066EPart0> Unknown1 { get; set; } = [];
}
