using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

[Message(GameMessageOpcode.ClientNonSpellActionSetChanges)]
[WireMessage(GameMessageOpcode.ClientNonSpellActionSetChanges, Direction.ClientToServer)]
public sealed partial class ClientNonSpellActionSetChanges : IReadable
{
    [UInt(6)] public UILocation ActionBarIndex { get; private set; }
    [UInt(4)] public ShortcutType ShortcutType { get; private set; }
    [UInt(32)] public uint ObjectId { get; private set; } // depending on ShortcutType can be Spell4Id, Item2Id, GameCommandType
    [UInt(4)] public byte SpecIndex { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ActionBarIndex = reader.ReadEnum<UILocation>(6u);
        ShortcutType = reader.ReadEnum<ShortcutType>(4u);
        ObjectId = reader.ReadUInt();
        SpecIndex = reader.ReadByte(4u);
    }
}
