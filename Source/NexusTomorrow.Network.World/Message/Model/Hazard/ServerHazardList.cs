using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

[WireMessage(GameMessageOpcode.Message010A, Direction.ServerToClient)]
public sealed partial class Message010A
{
    [Array(CountBits = 32)] public List<Message010APart0> Unknown0 { get; set; } = [];
}
