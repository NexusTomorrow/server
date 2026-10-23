using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Adds or updates recruiting guilds in the client's list.
/// </summary>
/// <remarks>
/// The client raises RecruitmentGuildsUpdated.
/// </remarks>
[WireMessage(GameMessageOpcode.Server076B, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildRecruitmentListUpdate
{
    /// <summary>
    /// One recruiting guild.
    /// </summary>
    [WireStruct]
    public sealed partial class Listing
    {
        /// <summary>
        /// The guild, by its id within the realm.
        /// </summary>
        [UInt(64)] public ulong GuildId { get; set; }

        /// <summary>
        /// The guild's name.
        /// </summary>
        [WideString] public string GuildName { get; set; } = "";

        /// <summary>
        /// The guild master, by name.
        /// </summary>
        [WideString] public string LeaderName { get; set; } = "";

        /// <summary>
        /// The guild's size, perks and classification.
        /// </summary>
        public GuildRecruitmentSummary Summary { get; set; } = new();
    }

    /// <summary>
    /// The guilds to add or update.
    /// </summary>
    [Array(CountBits = 32)] public List<Listing> Guilds { get; set; } = [];
}
