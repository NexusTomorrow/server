using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Updates what the player has withdrawn from the guild bank today.
/// </summary>
/// <remarks>
/// The client raises GuildBankWithdraw.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04A1, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildBankWithdrawalsChanged
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; private set; } = new();

    /// <summary>
    /// What the player has withdrawn today.
    /// </summary>
    public GuildBankWithdrawals Withdrawals { get; private set; } = new();
}
