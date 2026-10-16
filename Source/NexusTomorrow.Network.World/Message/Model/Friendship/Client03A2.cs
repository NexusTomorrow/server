using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

[WireMessage(GameMessageOpcode.Client03A2, Direction.ClientToServer)]
public sealed partial class Client03A2
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
}
