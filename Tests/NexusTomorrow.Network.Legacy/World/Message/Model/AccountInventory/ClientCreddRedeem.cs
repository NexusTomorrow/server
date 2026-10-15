using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[Message(GameMessageOpcode.ClientCreddRedeem)]
public class ClientCreddRedeem : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
