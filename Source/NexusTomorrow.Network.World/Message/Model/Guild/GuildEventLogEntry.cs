using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

/// <summary>
/// One entry in a guild's event log or bank log.
/// </summary>
[WireStruct]
public sealed partial class GuildEventLogEntry
{
    /// <summary>
    /// The entry's id, which the client files it under.
    /// </summary>
    [UInt(64)] public ulong Id { get; set; }

    /// <summary>
    /// What happened, a GuildEventType value. In the bank log, 1000 is a money deposit, 1001 a withdrawal and 2000 a repair, and
    /// 100000 plus a tab times 1000 an item deposited in that tab (plus 0) or withdrawn (plus 1).
    /// </summary>
    [UInt(17)] public uint Type { get; set; }

    /// <summary>
    /// Whom or what the entry is about, by type: a member's character id, an achievement or a perk.
    /// </summary>
    [UInt(64)] public ulong SubjectId { get; set; }

    /// <summary>
    /// The entry's first value, by type: an item's id, an amount of money, or the old rank of a rank change.
    /// </summary>
    [UInt(64)] public ulong Value1 { get; set; }

    /// <summary>
    /// The entry's second value, by type: an item's stack count, or the new rank of a rank change.
    /// </summary>
    [UInt(64)] public ulong Value2 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(64)] public ulong Unknown5 { get; set; }

    /// <summary>
    /// When it happened, in days from now: negative in the past.
    /// </summary>
    [Float32] public float Time { get; set; }
}
