using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellList, Direction.ServerToClient)]
public sealed partial class ServerSpellList
{
    [Array(CountBits = 32)] public List<SpellInit> SpellInitData { get; } = [];
}

