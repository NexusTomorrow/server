using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireMessage(GameMessageOpcode.ServerSpellUpdateEffectDuration, Direction.ServerToClient)]
public sealed partial class ServerSpellUpdateEffectDuration
{
    [UInt(32)] public uint ServerUniqueId { get; set; }
    [UInt(32)] public uint SpellEffectUniqueId { get; set; }
    [UInt(32)] public int TimeRemaining { get; set; }
    [UInt(32)] public uint TargetUnitId { get; set; }
}
