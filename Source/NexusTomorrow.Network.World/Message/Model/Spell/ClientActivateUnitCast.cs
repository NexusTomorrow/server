using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ClientActivateUnitCast)]
[WireMessage(GameMessageOpcode.ClientActivateUnitCast, Direction.ClientToServer)]
public sealed partial class ClientActivateUnitCast : IReadable
{
    [UInt(32)] public uint ClientSpellCastUniqueId { get; private set; }
    [UInt(32)] public uint UnitId { get; private set; } // Unit to activate

    public void Read(GamePacketReader reader)
    {
        ClientSpellCastUniqueId = reader.ReadUInt();
        UnitId = reader.ReadUInt();
    }
}
