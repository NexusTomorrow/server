using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellVisualListAdd)]
[WireMessage(GameMessageOpcode.ServerSpellVisualListAdd, Direction.ServerToClient)]
public sealed partial class ServerSpellVisualListAdd : IWritable
{
    [Array(CountBits = 32)] public List<ServerSpellVisualAdd> SpellVisualList { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SpellVisualList.Count);
        foreach (ServerSpellVisualAdd item in SpellVisualList)
        {
            item.Write(writer);
        }
    }
}
