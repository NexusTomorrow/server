using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.ICComm;

// Sent when client receives an ICComm message to a channel it is not in
// Presumably to let server know to resend a message if the server thought the client was in the channel
[WireMessage(GameMessageOpcode.ClientICCommChannelNotJoined, Direction.ClientToServer)]
public sealed partial class ClientICCommChannelNotJoined
{
    [UInt(64)] public ulong IccomId { get; private set; }
}
