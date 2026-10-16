using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Client00DE, Direction.ClientToServer)]
public sealed partial class Client00DE
{
    [UInt(3)] public byte Unknown0 { get; set; }
}
