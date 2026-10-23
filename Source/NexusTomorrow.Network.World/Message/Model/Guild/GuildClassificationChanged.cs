using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sets a guild's classification.
/// </summary>
/// <remarks>
/// GuildTypeLib's GetGuildClassification returns it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0480, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildClassificationChanged
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The guild's classification, a GuildClassification value.
    /// </summary>
    [UInt(3)] public byte Classification { get; set; }
}
