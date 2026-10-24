using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Lists every guild the player belongs to.
/// </summary>
/// <remarks>
/// The client drops any guild it holds that the list leaves out, then raises GuildChange, and GuildMessageOfTheDay for each
/// guild with a message of the day.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0497, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GuildMemberships
{
    /// <summary>
    /// How many guilds each of the three lists holds.
    /// </summary>
    [UInt(32), Count] public uint GuildCount { get; set; }

    /// <summary>
    /// The guild the player shows on their nameplate, as an index into <see cref="Guilds"/>.
    /// </summary>
    [UInt(32)] public uint NameplateIndex { get; set; }

    /// <summary>
    /// The player's guilds.
    /// </summary>
    [Array(CountFrom = nameof(GuildCount))] public List<GuildRecord> Guilds { get; set; } = [];

    /// <summary>
    /// The player's own member record in each guild, in the same order.
    /// </summary>
    [Array(CountFrom = nameof(GuildCount))] public List<GuildMember> Members { get; set; } = [];

    /// <summary>
    /// What the player has withdrawn today from each guild's bank, in the same order.
    /// </summary>
    [Array(CountFrom = nameof(GuildCount))] public List<GuildBankWithdrawals> Withdrawals { get; set; } = [];
}
