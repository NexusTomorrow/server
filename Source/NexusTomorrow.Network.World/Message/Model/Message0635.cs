using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Message0635, Direction.Both)]
public sealed partial class Message0635
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
