using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Message062A, Direction.Both)]
[Dormant]
public sealed partial class Message062A
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
