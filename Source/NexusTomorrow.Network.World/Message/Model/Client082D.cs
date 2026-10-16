using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client082D, Direction.ClientToServer)]
public sealed partial class Client082D
{
    [UInt(14)] public ushort Unknown0 { get; private set; }
}
