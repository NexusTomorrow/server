using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// Removes all spell visual effects from a unit
[Message(GameMessageOpcode.ServerSpellVisualEffectRemoveFromUnit)]
[WireMessage(GameMessageOpcode.ServerSpellVisualEffectRemoveFromUnit, Direction.ServerToClient)]
public sealed partial class ServerSpellVisualEffectRemoveFromUnit : IWritable
{
    [UInt(18)] public uint TargetUnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(TargetUnitId, 18u);
    }
}

