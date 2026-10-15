using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[Message(GameMessageOpcode.ClientCreddHistoryRequest)]
public class ClientCreddHistoryRequest : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
