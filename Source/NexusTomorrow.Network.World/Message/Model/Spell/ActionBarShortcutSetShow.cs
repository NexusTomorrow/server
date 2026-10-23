using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Shows an action bar's shortcut set.
/// </summary>
/// <remarks>
/// The client fills the bar from the set's twelve slots and raises ShowActionBarShortcut.
/// </remarks>
[WireMessage(GameMessageOpcode.Server016C, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class ActionBarShortcutSetShow
{
    /// <summary>
    /// The bar.
    /// </summary>
    [UInt(4)] public byte Bar { get; set; }

    /// <summary>
    /// The shortcut set, an ActionBarShortcutSet table id.
    /// </summary>
    [UInt(14)] public ushort ShortcutSetId { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }
}
