using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ClientCastBindPointSpell)]
[WireMessage(GameMessageOpcode.ClientCastBindPointSpell, Direction.ClientToServer)]
public sealed partial class ClientCastBindPointSpell : IReadable
{
    [UInt(32)] public uint ClientSpellCastUniqueId { get; private set; }
    [UInt(32)] public uint UnitId { get; private set; } // Bind point units

    public void Read(GamePacketReader reader)
    {
        ClientSpellCastUniqueId = reader.ReadUInt();
        UnitId = reader.ReadUInt();
    }
}
