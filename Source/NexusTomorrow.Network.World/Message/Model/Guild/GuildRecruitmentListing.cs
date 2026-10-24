using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// One recruiting guild.
/// </summary>
[WireStruct]
public sealed partial class GuildRecruitmentListing
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

    [Array(CountBits = 32), WideString] public List<string> Unknown4 { get; set; } = [];
}
