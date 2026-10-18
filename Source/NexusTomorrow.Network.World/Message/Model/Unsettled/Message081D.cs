using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Message081D, Direction.Both)]
public sealed partial class Message081D
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
