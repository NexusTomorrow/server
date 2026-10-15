using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

public class AccountItemCooldown : IWritable
{
    public uint AccountItemCooldownGroup { get; set; }
    public uint CooldownInSeconds { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AccountItemCooldownGroup);
        writer.Write(CooldownInSeconds);
    }
}
