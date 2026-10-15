using NexusTomorrow.Game.Static;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory
{
    [Message(GameMessageOpcode.ServerAccountTier)]
    public class ServerAccountTier : IWritable
    {
        public AccountTier Tier { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Tier, 5u);
        }
    }
}
