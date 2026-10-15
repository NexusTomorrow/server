using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

[WireMessage(GameMessageOpcode.ServerAccountCurrencies, Direction.ServerToClient)]
public sealed partial class ServerAccountCurrencies
{
    [Array(CountBits = 32)] public List<AccountCurrency> AccountCurrencies { get; set; } = [];
}
