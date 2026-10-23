using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets a guild's ranks.
/// </summary>
/// <remarks>
/// The client raises GuildRankChange.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04C5, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildRanksChanged
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The guild's ten ranks, in rank order.
    /// </summary>
    [FixedArray(10)] public GuildRank[] Ranks { get; set; } = [.. Enumerable.Range(0, 10).Select(_ => new GuildRank())];
}
