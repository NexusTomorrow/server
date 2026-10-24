using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Adds a guild the player belongs to.
/// </summary>
/// <remarks>
/// The client raises GuildChange, and GuildMessageOfTheDay when the guild has a message of the day.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0493, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GuildJoined
{
    /// <summary>
    /// The guild.
    /// </summary>
    public GuildRecord Guild { get; set; } = new();

    /// <summary>
    /// The player's own member record in the guild.
    /// </summary>
    public GuildMember Member { get; set; } = new();

    /// <summary>
    /// What the player has withdrawn from the guild's bank today.
    /// </summary>
    public GuildBankWithdrawals Withdrawals { get; set; } = new();

    /// <summary>
    /// True when the player shows this guild on their nameplate, which GuildTypeLib's IsNameplate returns.
    /// </summary>
    [UInt(1)] public bool IsNameplate { get; set; }
}
