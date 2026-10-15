using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ClientCastGuildBossToken)]
[WireMessage(GameMessageOpcode.ClientCastGuildBossToken, Direction.ClientToServer)]
public sealed partial class ClientCastGuildBossToken : IReadable
{
    public Identity GuildIdentity { get; private set; } = new Identity();
    [UInt(32)] public uint Item2Id { get; private set; } // item2Id of the guild boss token item being used
    [UInt(32)] public uint ClientSpellCastUniqueId { get; private set; }

    public void Read(GamePacketReader reader)
    {
        GuildIdentity.Read(reader);
        Item2Id = reader.ReadUInt();
        ClientSpellCastUniqueId = reader.ReadUInt();
    }
}
