using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

// Sent by client if inventory has not been loaded when the tab is opened.
// Server sends ServerGuildBankTabInventory (0x47A) response
[WireMessage(GameMessageOpcode.ClientGuildBankTabOpen, Direction.ClientToServer)]
public sealed partial class ClientGuildBankTabOpen
{
    public Identity GuildIdentity { get; private set; } = new();
    [UInt(9)] public ushort BankTabIndex { get; private set; } // Guild bank tab indexes are 100-109, WarParty bank tabs 200-209
}
