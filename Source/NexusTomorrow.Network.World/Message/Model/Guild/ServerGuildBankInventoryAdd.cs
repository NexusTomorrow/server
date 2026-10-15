using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ServerGuildBankInventoryAdd, Direction.ServerToClient)]
public sealed partial class ServerGuildBankInventoryAdd
{
    public Identity GuildIdentity { get; set; }
    public InventoryItem InventoryItem { get; set; } // Make sure ItemLocation is a GuildBankTab
}
