using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client00C8, Direction.ClientToServer)]
public sealed partial class Client00C8
{
    [UInt(5)] public byte Unknown0 { get; set; }
}
