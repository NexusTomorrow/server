using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[Message(GameMessageOpcode.ClientCreddHistoryRequest)]
[WireMessage(GameMessageOpcode.ClientCreddHistoryRequest, Direction.ClientToServer)]
public sealed partial class ClientCreddHistoryRequest : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
