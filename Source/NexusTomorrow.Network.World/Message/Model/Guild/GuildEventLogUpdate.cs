using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Adds an entry to a guild's event log, or replaces the entry with its id.
/// </summary>
/// <remarks>
/// The client files the entry by its id and raises GuildEventLogChange.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0487, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildEventLogUpdate
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The entry.
    /// </summary>
    public GuildEventLogEntry Entry { get; set; } = new();
}
