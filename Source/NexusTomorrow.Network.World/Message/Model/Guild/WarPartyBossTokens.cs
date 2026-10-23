using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Lists a war party's boss tokens.
/// </summary>
/// <remarks>
/// The client replaces the tokens it holds and raises WarPartyBossTokensUpdated.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0951, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class WarPartyBossTokens
{
    /// <summary>
    /// The war party, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The war party's boss tokens.
    /// </summary>
    [Array(CountBits = 32)] public List<WarPartyBossToken> Tokens { get; set; } = [];
}
