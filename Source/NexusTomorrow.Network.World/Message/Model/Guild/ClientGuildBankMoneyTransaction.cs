using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.ClientGuildBankMoneyTransaction, Direction.ClientToServer)]
public sealed partial class ClientGuildBankMoneyTransaction
{
    public Identity GuildIdentity { get; private set; } = new();
    [UInt(64)] public long Amount { get; private set; } // Negative for deposit, positive for withdraw
}
