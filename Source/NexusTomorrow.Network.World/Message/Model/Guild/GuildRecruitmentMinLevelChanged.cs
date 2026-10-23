using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets the lowest level a recruiting guild takes.
/// </summary>
/// <remarks>
/// GuildTypeLib's GetRecruitmentMinLevel returns it; the client raises RecruitmentMinLevel.
/// </remarks>
[WireMessage(GameMessageOpcode.Server076C, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildRecruitmentMinLevelChanged
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The lowest level the guild recruits.
    /// </summary>
    [UInt(32)] public uint MinLevel { get; set; }
}
