using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientMatchingTransferIntoMatch)]
[WireMessage(GameMessageOpcode.ClientMatchingTransferIntoMatch, Direction.ClientToServer)]
public sealed partial class ClientMatchingTransferIntoMatch : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // deliberately empty
    }
}
