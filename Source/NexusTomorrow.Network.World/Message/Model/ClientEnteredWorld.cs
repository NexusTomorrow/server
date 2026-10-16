using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client00F2, Direction.ClientToServer)]
public sealed partial class Client00F2
{
    [UInt(15)] public ushort Unknown0 { get; private set; }
}
