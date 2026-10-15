using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.Interrupted)]
public sealed partial class CombatLogInterrupted : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.Interrupted;

    [UInt(18)] public uint InterruptingSpellId { get; set; } // 18u
    [UInt(9)] public CastResult CastResult { get; set; } // 9u
    public CombatLogCastData CastData { get; set; }
}
