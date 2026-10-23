using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Updates a guild's PvP record.
/// </summary>
/// <remarks>
/// GuildTypeLib's GetPvpRatings reports the four values as nWins, nLosses, nDraws and nRating.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04C1, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildPvpRatingChanged
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The guild's wins.
    /// </summary>
    [UInt(32)] public uint Wins { get; set; }

    /// <summary>
    /// The guild's losses.
    /// </summary>
    [UInt(32)] public uint Losses { get; set; }

    /// <summary>
    /// The guild's draws.
    /// </summary>
    [UInt(32)] public uint Draws { get; set; }

    /// <summary>
    /// The guild's rating.
    /// </summary>
    [UInt(32)] public uint Rating { get; set; }
}
