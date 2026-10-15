using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory
{
    [Message(GameMessageOpcode.ClientDailyLoginRewardsRequest)]
    public class ClientDailyLoginRewardsRequest : IReadable
    {
        public void Read(GamePacketReader reader)
        {
            // zero byte message
        }
    }
}
