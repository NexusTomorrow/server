using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[Message(GameMessageOpcode.ServerSpellCooldownStart)]
public class ServerSpellCooldownStart : IWritable
{
    public Cooldown Cooldown { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Cooldown.Write(writer);
    }
}
