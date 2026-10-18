using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Server019C, Direction.ServerToClient)]
public sealed partial class Server019C
{
    [Array(CountBits = 7), UInt(16)] public List<ushort> Unknown0 { get; } = [];
}
