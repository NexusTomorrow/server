using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellVisualEffectRemoveList)]
[WireMessage(GameMessageOpcode.ServerSpellVisualEffectRemoveList, Direction.ServerToClient)]
public sealed partial class ServerSpellVisualEffectRemoveList : IWritable
{
    [Array(CountBits = 32), UInt(32)] public List<uint> SpellVisualEffectUniqueIds { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SpellVisualEffectUniqueIds.Count);
        SpellVisualEffectUniqueIds.ForEach(id => writer.Write(id));
    }
}
