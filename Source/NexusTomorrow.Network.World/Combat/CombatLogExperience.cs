using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.Experience)]
public sealed partial class CombatLogExperience : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.Experience;

    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public uint Xp { get; set; }
    [UInt(32)] public uint RestXp { get; set; }
    [UInt(32)] public uint ElderPoints { get; set; }
    [UInt(32)] public uint RestElderPoints { get; set; }
}
