using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerAccountCurrencyGrant, Direction.ServerToClient)]
public sealed partial class ServerAccountCurrencyGrant
{
    public AccountCurrency AccountCurrency { get; set; }
    [UInt(64)] public ulong NewAmount { get; set; }
    [UInt(64)] public ulong SignatureBonusAmount { get; set; }
    [UInt(64)] public ulong EssenceBonusAmount { get; set; }
}
