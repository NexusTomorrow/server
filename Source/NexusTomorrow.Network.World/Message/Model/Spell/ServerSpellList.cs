using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellList)]
public class ServerSpellList : IWritable
{
    public List<SpellInit> SpellInitData { get; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SpellInitData.Count);
        foreach (SpellInit data in SpellInitData)
        {
            data.Write(writer);
        }
    }
}

