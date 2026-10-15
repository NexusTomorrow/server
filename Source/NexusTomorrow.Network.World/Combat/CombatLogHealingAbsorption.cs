using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.HealingAbsorption)]
public sealed partial class CombatLogHealingAbsorption : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.HealingAbsorption;

    [UInt(32)] public uint Amount { get; set; }
}
