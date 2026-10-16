using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client03A7, Direction.ClientToServer)]
public sealed partial class Client03A7
{
    [UInt(3)] public byte Unknown0 { get; private set; }
}
