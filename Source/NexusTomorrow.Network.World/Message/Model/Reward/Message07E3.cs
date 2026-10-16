using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.Message07E3, Direction.Both)]
public sealed partial class Message07E3
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
