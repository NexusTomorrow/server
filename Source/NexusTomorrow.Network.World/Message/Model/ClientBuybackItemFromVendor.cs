using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client00BB, Direction.ClientToServer)]
public sealed partial class Client00BB
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
