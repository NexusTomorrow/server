using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.Resurrect)]
public sealed partial class CombatLogResurrect : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.Resurrect;

    [UInt(32)] public uint UnitId { get; set; }
}
