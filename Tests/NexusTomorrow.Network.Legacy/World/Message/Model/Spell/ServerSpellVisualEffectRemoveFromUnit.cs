using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// Removes all spell visual effects from a unit
[Message(GameMessageOpcode.ServerSpellVisualEffectRemoveFromUnit)]
public class ServerSpellVisualEffectRemoveFromUnit : IWritable
{
    public uint TargetUnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TargetUnitId, 18u);
    }
}

