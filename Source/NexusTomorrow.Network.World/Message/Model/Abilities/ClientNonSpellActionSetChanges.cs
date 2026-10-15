using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[WireMessage(GameMessageOpcode.ClientNonSpellActionSetChanges, Direction.ClientToServer)]
public sealed partial class ClientNonSpellActionSetChanges
{
    [UInt(6)] public UILocation ActionBarIndex { get; private set; }
    [UInt(4)] public ShortcutType ShortcutType { get; private set; }
    [UInt(32)] public uint ObjectId { get; private set; } // depending on ShortcutType can be Spell4Id, Item2Id, GameCommandType
    [UInt(3)] public byte SpecIndex { get; private set; }
}
