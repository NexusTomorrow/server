using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sends a recruiting guild's details.
/// </summary>
/// <remarks>
/// The client raises RecruitmentDetailedGuildInfo.
/// </remarks>
[WireMessage(GameMessageOpcode.Server076A, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GuildRecruitmentDetails
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    [WideString] public string Unknown1 { get; set; } = "";

    [Float32] public float Unknown2 { get; set; }

    [UInt(1)] public bool Unknown3 { get; set; }

    [UInt(32)] public uint Unknown4 { get; set; }

    /// <summary>
    /// What the guild asks for.
    /// </summary>
    public GuildRecruitmentDemands Demands { get; set; } = new();
}
