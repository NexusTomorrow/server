using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Sets the amounts of the account's currencies; the client raises AccountCurrencyChanged.
/// </summary>
[WireMessage(GameMessageOpcode.Server0966, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AccountCurrencies
{
    /// <summary>
    /// The currencies, each with the amount the account holds.
    /// </summary>
    [Array(CountBits = 32)] public List<AccountCurrency> Currencies { get; set; } = [];
}
