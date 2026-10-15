using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellVisualEffectRemoveList)]
public class ServerSpellVisualEffectRemoveList : IWritable
{
    public List<uint> SpellVisualEffectUniqueIds { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SpellVisualEffectUniqueIds.Count);
        SpellVisualEffectUniqueIds.ForEach(id => writer.Write(id));
    }
}
