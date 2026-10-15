using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireStruct]
public sealed partial class AccountCurrency
{
    [UInt(5)] public AccountCurrencyType Type { get; set; }
    [UInt(64)] public ulong Amount { get; set; }
}
