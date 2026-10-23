using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Fills an action bar's shortcuts.
/// </summary>
/// <remarks>
/// The client marks the bar visible when any slot is set, and raises ShowActionBarShortcut.
/// </remarks>
[WireMessage(GameMessageOpcode.Server016B, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ActionBarShortcutsSet
{
    /// <summary>
    /// The bar.
    /// </summary>
    [UInt(4)] public byte Bar { get; set; }

    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The number of slots listed.
    /// </summary>
    [UInt(4), Count] public byte Count { get; set; }

    /// <summary>
    /// Each slot's object, by id; the slot's shortcut type says what the id names.
    /// </summary>
    [Array(CountFrom = nameof(Count)), UInt(32)] public List<uint> ObjectIds { get; } = [];

    /// <summary>
    /// Each slot's shortcut type.
    /// </summary>
    [Array(CountFrom = nameof(Count)), UInt(32)] public List<uint> ShortcutTypes { get; } = [];
}
