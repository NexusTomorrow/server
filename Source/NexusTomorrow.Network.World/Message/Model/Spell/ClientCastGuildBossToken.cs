using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ClientCastGuildBossToken, Direction.ClientToServer)]
public sealed partial class ClientCastGuildBossToken
{
    public Identity GuildIdentity { get; private set; } = new Identity();
    [UInt(32)] public uint Item2Id { get; private set; } // item2Id of the guild boss token item being used
    [UInt(32)] public uint ClientSpellCastUniqueId { get; private set; }
}
