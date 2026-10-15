using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[Message(GameMessageOpcode.ServerGuildBankTabInventory)]
public class ServerGuildBankTabInventory : IWritable
{
    public Identity GuildIdentity { get; set; }
    public InventoryLocation BankTabIndex { get; set; } // Guild bank tab indexes are 100-109, WarParty bank tabs 200-209
    public List<InventoryItem> InventoryItems { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        GuildIdentity.Write(writer);
        writer.Write(BankTabIndex, 9u);
        foreach (InventoryItem item in InventoryItems)
        {
            item.Write(writer);
        }
    }
}
