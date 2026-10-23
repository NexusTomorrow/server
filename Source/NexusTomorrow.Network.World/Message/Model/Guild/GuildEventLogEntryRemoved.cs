using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Removes an entry from a guild's event log.
/// </summary>
[WireMessage(GameMessageOpcode.Server048A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GuildEventLogEntryRemoved
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    /// <summary>
    /// The entry, by its id in the log.
    /// </summary>
    [UInt(64)] public ulong EntryId { get; set; }
}
