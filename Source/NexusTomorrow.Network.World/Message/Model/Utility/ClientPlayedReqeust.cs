using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Utility;

[Message(GameMessageOpcode.ClientPlayedRequest)]
[WireMessage(GameMessageOpcode.ClientPlayedRequest, Direction.ClientToServer)]
public sealed partial class ClientPlayedReqeust : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // packet has no payload
    }
}
