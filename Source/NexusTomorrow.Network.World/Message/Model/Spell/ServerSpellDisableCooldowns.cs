using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// Resets all cooldowns and gives spells infinite charges
[Message(GameMessageOpcode.ServerSpellDisableCooldowns)]
public class ServerSpellDisableCooldowns : IWritable
{
    public bool DisableCooldowns { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(DisableCooldowns);
    }
}
