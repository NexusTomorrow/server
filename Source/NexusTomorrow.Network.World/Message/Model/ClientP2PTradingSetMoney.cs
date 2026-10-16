using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client0197, Direction.ClientToServer)]
public sealed partial class Client0197
{
    [UInt(64)] public ulong Unknown0 { get; private set; }
}
