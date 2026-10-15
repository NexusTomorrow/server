using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[Message(GameMessageOpcode.ServerShowActionBar)]
[WireMessage(GameMessageOpcode.ServerShowActionBar, Direction.ServerToClient)]
public sealed partial class ServerShowActionBar : IWritable
{
    [UInt(4)] public ShortcutSet ShortcutSet { get; set; }
    [UInt(14)] public ushort ActionBarShortcutSetId { get; set; }
    [UInt(32)] public uint AssociatedUnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ShortcutSet, 4u);
        writer.Write(ActionBarShortcutSetId, 14u);
        writer.Write(AssociatedUnitId);
    }
}
