using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// One realm in a realm list.
/// </summary>
[WireStruct]
public partial class RealmListEntry
{
    /// <summary>
    /// Values the client shows with a realm.
    /// </summary>
    [WireStruct]
    public sealed partial class RealmSummary
    {
        /// <summary>
        /// The client gives this value no meaning.
        /// </summary>
        [UInt(14)] public ushort Unknown0 { get; set; }

        /// <summary>
        /// A count the client shows with the realm.
        /// </summary>
        [UInt(32)] public uint Count { get; set; }

        /// <summary>
        /// A character name the client shows as last played on the realm, when present.
        /// </summary>
        [WideString] public string LastPlayed { get; set; } = "";

        /// <summary>
        /// The client gives this value no meaning.
        /// </summary>
        [UInt(64)] public ulong Unknown3 { get; set; }
    }

    /// <summary>
    /// The realm's id; <see cref="RealmChoice"/> names a realm by it.
    /// </summary>
    [UInt(32)] public uint RealmId { get; set; }

    /// <summary>
    /// The realm's name, shown to the player.
    /// </summary>
    [WideString] public string Name { get; set; } = "";

    /// <summary>
    /// A note about the realm, as an id in the client's text table, shown to the player.
    /// </summary>
    [UInt(32)] public uint NoteTextId { get; set; }

    /// <summary>
    /// The realm's flags. Bit 4 set means the realm keeps characters to one faction; bit 0 marks the realm the client picks when it selects
    /// one on its own. The client tests no other bit.
    /// </summary>
    [UInt(32)] public uint Flags { get; set; }

    /// <summary>
    /// The realm's PvP type: 0 PvE, 1 PvP. The interface treats any other value as PvE.
    /// </summary>
    [UInt(2)] public byte PvpType { get; set; }

    /// <summary>
    /// The realm's status: 0 unknown, 1 offline, 2 down, 3 on standby, 4 up. The interface shows any other value as unknown.
    /// </summary>
    [UInt(3)] public byte Status { get; set; }

    /// <summary>
    /// The realm's population: 0 low, 1 medium, 2 high, 3 full. The interface labels no other value.
    /// </summary>
    [UInt(3)] public byte Population { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown7 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [FixedArray(16), UInt(8)] public byte[] Unknown8 { get; set; } = new byte[16];

    /// <summary>
    /// Values the client shows with the realm.
    /// </summary>
    public RealmSummary Summary { get; set; } = new();

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(16)] public ushort Unknown10 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(16)] public ushort Unknown11 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(16)] public ushort Unknown12 { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(16)] public ushort Unknown13 { get; set; }
}
