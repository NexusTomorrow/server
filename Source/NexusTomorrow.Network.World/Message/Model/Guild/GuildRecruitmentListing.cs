using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// One recruiting guild, as RecruitmentGetActiveGuilds reports it.
/// </summary>
[WireStruct]
public sealed partial class GuildRecruitmentListing
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

    /// <summary>
    /// The guild's recruiters, by name.
    /// </summary>
    [Array(CountBits = 32), WideString] public List<string> Recruiters { get; set; } = [];
}
