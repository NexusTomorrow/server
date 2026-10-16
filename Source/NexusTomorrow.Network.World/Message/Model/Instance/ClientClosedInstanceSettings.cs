using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.Client00D2, Direction.ClientToServer)]
public sealed partial class Client00D2
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
