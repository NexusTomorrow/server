using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.ServerShowActionBar, Direction.ServerToClient)]
public sealed partial class ServerShowActionBar
{
    [UInt(4)] public ShortcutSet ShortcutSet { get; set; }
    [UInt(14)] public ushort ActionBarShortcutSetId { get; set; }
    [UInt(32)] public uint AssociatedUnitId { get; set; }
}
