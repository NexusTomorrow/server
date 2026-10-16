using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client07A5, Direction.ClientToServer)]
public sealed partial class Client07A5
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
