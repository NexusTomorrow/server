using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client00FB, Direction.ClientToServer)]
public sealed partial class Client00FB
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
