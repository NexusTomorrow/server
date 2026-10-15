using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

public class AccountCurrency : IWritable
{
    public AccountCurrencyType Type { get; set; }
    public ulong Amount { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Type, 5u);
        writer.Write(Amount);
    }
}
