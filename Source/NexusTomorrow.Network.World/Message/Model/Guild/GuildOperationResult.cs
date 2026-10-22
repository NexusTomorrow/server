using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// The result of a guild operation.
/// </summary>
/// <remarks>
/// The client raises GuildResult with the guild it holds under <see cref="GuildId"/>, if any, and the three other fields.
/// </remarks>
[WireMessage(GameMessageOpcode.Server04C9, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public partial class GuildOperationResult
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// A rank, from 0; the event passes it plus one as nRank.
    /// </summary>
    [UInt(32)] public uint RankIndex { get; set; }

    /// <summary>
    /// The name the event passes as strTarget.
    /// </summary>
    [WideString] public string TargetName { get; set; } = "";

    /// <summary>
    /// The code the event passes as eResult.
    /// </summary>
    [UInt(8)] public byte Result { get; set; }
}
