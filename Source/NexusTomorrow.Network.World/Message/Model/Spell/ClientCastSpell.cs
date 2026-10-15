using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ClientCastSpell)]
[WireMessage(GameMessageOpcode.ClientCastSpell, Direction.ClientToServer)]
public sealed partial class ClientCastSpell : IReadable
{
    [UInt(32)] public uint ClientSpellCastUniqueId { get; private set; } // first value of 0x7FD response, probably global increment
    [UInt(16)] public ushort BagIndex { get; private set; }
    [UInt(32)] public uint TargetUnitId { get; private set; }
    [UInt(1)] public bool ButtonPressed { get; private set; }

    public void Read(GamePacketReader reader)
    {
        ClientSpellCastUniqueId = reader.ReadUInt();
        BagIndex = reader.ReadUShort();
        TargetUnitId = reader.ReadUInt();
        ButtonPressed = reader.ReadBit();
    }
}
