using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sends a recruiting guild's details.
/// </summary>
/// <remarks>
/// The client raises RecruitmentDetailedGuildInfo; RecruitmentGetDetailedGuildInfo reports the details.
/// </remarks>
[WireMessage(GameMessageOpcode.Server076A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildRecruitmentDetails
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The guild's recruitment description.
    /// </summary>
    [WideString] public string Description { get; set; } = "";

    /// <summary>
    /// When the guild was created, in days from now.
    /// </summary>
    [Float32] public float CreationDate { get; set; }

    /// <summary>
    /// Whether the guild taxes its members.
    /// </summary>
    [UInt(1)] public bool Tax { get; set; }

    /// <summary>
    /// The lowest level the guild recruits.
    /// </summary>
    [UInt(32)] public uint MinLevel { get; set; }

    /// <summary>
    /// What the guild asks for.
    /// </summary>
    public GuildRecruitmentDemands Demands { get; set; } = new();
}
