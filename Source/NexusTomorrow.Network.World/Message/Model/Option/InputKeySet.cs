using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Option;

[WireMessage(GameMessageOpcode.Message056F, Direction.Both)]
public sealed partial class InputKeySet
{
    [Array(CountBits = 32)] public List<InputKeyBinding> Unknown0 { get; set; } = [];
    [UInt(64)] public ulong Unknown1 { get; set; }
}
