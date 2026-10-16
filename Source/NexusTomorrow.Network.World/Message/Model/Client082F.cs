using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client082F, Direction.ClientToServer)]
public sealed partial class Client082F
{
    [UInt(18)] public uint Unknown0 { get; private set; }
}
