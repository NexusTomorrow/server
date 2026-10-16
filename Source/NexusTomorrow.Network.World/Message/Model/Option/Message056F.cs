using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Option;

[WireMessage(GameMessageOpcode.Message056F, Direction.Both)]
public sealed partial class Message056F
{
    [Array(CountBits = 32)] public List<Message056FPart0> Unknown0 { get; set; } = [];
    [UInt(64)] public ulong Unknown1 { get; set; }
}
