using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellVisualListAdd, Direction.ServerToClient)]
public sealed partial class ServerSpellVisualListAdd
{
    [Array(CountBits = 32)] public List<ServerSpellVisualAdd> SpellVisualList { get; set; } = [];
}
