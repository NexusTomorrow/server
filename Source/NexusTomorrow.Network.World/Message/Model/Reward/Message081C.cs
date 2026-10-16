using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Message081C, Direction.Both)]
public sealed partial class Message081C
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
