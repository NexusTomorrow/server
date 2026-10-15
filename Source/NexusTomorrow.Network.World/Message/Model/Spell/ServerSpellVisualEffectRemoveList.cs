using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellVisualEffectRemoveList, Direction.ServerToClient)]
public sealed partial class ServerSpellVisualEffectRemoveList
{
    [Array(CountBits = 32), UInt(32)] public List<uint> SpellVisualEffectUniqueIds { get; set; } = [];
}
