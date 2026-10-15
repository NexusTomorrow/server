using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
public sealed partial class CombatLogKillStreak : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.KillStreak;

    [UInt(32)] public uint UnitId { get; set; }
    [UInt(5)] public CombatMomentumStat StatType { get; set; } // 5u
    [UInt(32)] public uint StreakAmount { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(StatType, 5u);
        writer.Write(StreakAmount);
    }
}
