using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets a guild's influence, money and war coins.
/// </summary>
/// <remarks>
/// The client raises GuildInfluenceAndMoney with the guild, the influence, the money and the bonus influence remaining, and
/// GuildWarCoinsChanged with the difference when the war coins changed.
/// </remarks>
[WireMessage(GameMessageOpcode.Server048D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildInfluenceAndMoney
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The guild's influence, the event's nInfluence.
    /// </summary>
    [UInt(32)] public uint Influence { get; set; }

    /// <summary>
    /// The bonus influence remaining, the event's nBonusInfluenceRemaining.
    /// </summary>
    [UInt(32)] public uint BonusInfluenceRemaining { get; set; }

    /// <summary>
    /// The guild's money, the event's monUpdated.
    /// </summary>
    [UInt(64)] public ulong Money { get; set; }

    /// <summary>
    /// The guild's war coins.
    /// </summary>
    [UInt(32)] public uint WarCoins { get; set; }
}
