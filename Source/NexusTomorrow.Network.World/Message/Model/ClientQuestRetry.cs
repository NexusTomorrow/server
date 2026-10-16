using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client0365, Direction.ClientToServer)]
public sealed partial class Client0365
{
    [UInt(15)] public ushort Unknown0 { get; private set; }
}
