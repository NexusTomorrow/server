using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.DurabilityLoss)]
public sealed partial class CombatLogDurabilityLoss : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.DurabilityLoss;

    [UInt(32)] public uint UnitId { get; set; }
    [Float32] public float Amount { get; set; }
}
