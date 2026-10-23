using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// Sends a guild's event log or its bank log.
/// </summary>
[WireMessage(GameMessageOpcode.Server0489, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class GuildEventLog
{
    /// <summary>
    /// The guild, by id.
    /// </summary>
    public RealmScopedId GuildId { get; set; } = new();

    [UInt(1)] public bool Unknown1 { get; set; }

    /// <summary>
    /// The log's entries.
    /// </summary>
    [Array(CountBits = 32)] public List<GuildEventLogEntry> Entries { get; set; } = [];
}
