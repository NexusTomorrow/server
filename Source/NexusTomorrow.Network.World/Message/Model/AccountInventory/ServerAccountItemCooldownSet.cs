using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory
{
    [Message(GameMessageOpcode.ServerAccountItemCooldownSet)]
    public class ServerAccountItemCooldownSet : IWritable
    {
        public AccountItemCooldown Cooldown { get; set; }

        public void Write(GamePacketWriter writer)
        {
            Cooldown.Write(writer);
        }
    }
}
