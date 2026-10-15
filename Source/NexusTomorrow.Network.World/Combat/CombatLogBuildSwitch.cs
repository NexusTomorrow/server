using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.BuildSwitch)]
public sealed partial class CombatLogBuildSwitch : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.BuildSwitch;

    [UInt(32)] public uint UnitId { get; set; }
    [UInt(3)] public byte NewSpecIndex { get; set; } // 3u
}
