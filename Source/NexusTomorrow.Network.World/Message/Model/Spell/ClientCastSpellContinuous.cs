using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ClientCastSpellContinuous)]
[WireMessage(GameMessageOpcode.ClientCastSpellContinuous, Direction.ClientToServer)]
public sealed partial class ClientCastSpellContinuous : IReadable
{
    [UInt(16)] public ushort BagIndex { get; private set; }
    [UInt(32)] public uint TargetUnitId { get; private set; }
    [UInt(1)] public bool ButtonPressed { get; private set; }

    public void Read(GamePacketReader reader)
    {
        BagIndex = reader.ReadUShort();
        TargetUnitId = reader.ReadUInt();
        ButtonPressed = reader.ReadBit();
    }
}
