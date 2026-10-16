using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[WireMessage(GameMessageOpcode.Message0550, Direction.Both)]
public sealed partial class Message0550
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
