using NexusTomorrow.Game.Static.AccountInventory;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// An amount of one account currency.
/// </summary>
[WireStruct]
public sealed partial class AccountCurrency
{
    /// <summary>
    /// The currency. In <see cref="AccountInventory.AccountCurrencies"/> a server sends only 0 and 4 to 18: the client keeps no
    /// currency above 18, and skips types 1 to 3, which it counts itself from the account items it holds.
    /// </summary>
    [UInt(5)] public AccountCurrencyType CurrencyType { get; set; }

    /// <summary>
    /// The amount of the currency. <see cref="AccountInventory.AccountCurrencies"/> sets the account's amount of it to this value.
    /// </summary>
    [UInt(64)] public ulong Amount { get; set; }
}
