using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

[WireMessage(GameMessageOpcode.Message010B, Direction.ServerToClient)]
public sealed partial class Message010B
{
    [Array(CountBits = 32)] public List<Message010BPart0> Unknown0 { get; set; } = [];
    [Array(CountBits = 32)] public List<Message010BPart0> Unknown1 { get; set; } = [];
}
