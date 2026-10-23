using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Asks for a war party's boss tokens: the request GuildTypeLib's ListBossTokens sends.
/// </summary>
/// <remarks>
/// The client sends it only for a war party whose tokens it does not hold already.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0956, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class GuildListBossTokens
{
    /// <summary>
    /// The war party, by id.
    /// </summary>
    public RealmScopedId GuildId { get; private set; } = new();
}
