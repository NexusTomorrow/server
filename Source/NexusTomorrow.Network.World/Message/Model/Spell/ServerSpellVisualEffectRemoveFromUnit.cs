using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// Removes all spell visual effects from a unit
[WireMessage(GameMessageOpcode.ServerSpellVisualEffectRemoveFromUnit, Direction.ServerToClient)]
public sealed partial class ServerSpellVisualEffectRemoveFromUnit
{
    [UInt(32)] public uint TargetUnitId { get; set; }
}

