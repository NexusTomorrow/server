using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildBankTabInventory, Direction.ServerToClient)]
public sealed partial class ServerGuildBankTabInventory
{
    public Identity GuildIdentity { get; set; }
    [UInt(9)] public InventoryLocation BankTabIndex { get; set; } // Guild bank tab indexes are 100-109, WarParty bank tabs 200-209
    [Array(CountBits = 32)] public List<InventoryItem> InventoryItems { get; set; } = [];
}
