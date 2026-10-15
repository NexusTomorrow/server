using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.Datacube)]
public sealed partial class CombatLogDatacube : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.Datacube;

    [UInt(32)] public uint UnitId { get; set; }
    [UInt(3)] public DatacubeType DatacubeType { get; set; } // 3u
    [UInt(1)] public bool HasPieces { get; set; }
}
