using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.WorldEntry;

[WireMessage(GameMessageOpcode.Client00BF, Direction.ClientToServer)]
public sealed partial class Client00BF
{
    [UInt(1)] public bool Unknown0 { get; private set; }
}
