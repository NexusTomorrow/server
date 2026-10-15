using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[Message(GameMessageOpcode.ClientCreddExchangeInfoRequest)]
[WireMessage(GameMessageOpcode.ClientCreddExchangeInfoRequest, Direction.ClientToServer)]
public sealed partial class ClientCreddExchangeInfoRequest : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
