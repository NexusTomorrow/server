using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.Client01A2, Direction.ClientToServer)]
public sealed partial class Client01A2
{
    [Array(CountBits = 7), UInt(16)] public List<ushort> Unknown0 { get; } = [];
}
