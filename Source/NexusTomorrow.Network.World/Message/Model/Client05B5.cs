using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client05B5, Direction.ClientToServer)]
public sealed partial class Client05B5
{
    [UInt(5)] public byte Unknown0 { get; private set; }
}
