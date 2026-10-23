using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Moves money into or out of the guild bank: the request GuildTypeLib's DepositMoney and WithdrawMoney send.
/// </summary>
[WireMessage(GameMessageOpcode.Client04A8, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GuildBankMoneyTransfer
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; private set; } = new();

    /// <summary>
    /// The amount: positive withdraws it, negative deposits it.
    /// </summary>
    [SInt(64)] public long Amount { get; private set; }
}
