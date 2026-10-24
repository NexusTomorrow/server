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
[Verified(Rung.Purpose)]
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

        [WideString] public string Unknown1 { get; set; } = "";

        [WideString] public string Unknown2 { get; set; } = "";

        /// <summary>
        /// The guild's summary record.
        /// </summary>
        public GuildRecruitmentSummary Summary { get; set; } = new();
    }

    /// <summary>
    /// The guilds to add or update.
    /// </summary>
    [Array(CountBits = 32)] public List<Listing> Guilds { get; set; } = [];
}
