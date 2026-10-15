using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ClientCastServiceToken)]
[WireMessage(GameMessageOpcode.ClientCastServiceToken, Direction.ClientToServer)]
public sealed partial class ClientCastServiceToken : IReadable
{
    [UInt(32)] public uint Spell4Id { get; private set; }
    [UInt(32)] public uint ClientSpellCastUniqueId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Spell4Id = reader.ReadUInt();
        ClientSpellCastUniqueId = reader.ReadUInt();
    }
}
