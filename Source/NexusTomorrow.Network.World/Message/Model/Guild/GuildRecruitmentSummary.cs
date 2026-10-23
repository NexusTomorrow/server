using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// A recruiting guild's size, perks and classification.
/// </summary>
[WireStruct]
public sealed partial class GuildRecruitmentSummary
{
    /// <summary>
    /// How many members the guild has.
    /// </summary>
    [UInt(16)] public ushort MemberCount { get; set; }

    /// <summary>
    /// How many perks the guild has.
    /// </summary>
    [UInt(16)] public ushort PerkCount { get; set; }

    /// <summary>
    /// The guild's classification, a GuildClassification value.
    /// </summary>
    [UInt(3)] public byte Classification { get; set; }
}
