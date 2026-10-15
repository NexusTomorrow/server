using NexusTomorrow.Game.Static.Combat;

namespace NexusTomorrow.Network.World.Combat
{
    public class CombatLogKillStreak : ICombatLog
    {
        public CombatLogType Type => CombatLogType.KillStreak;

        public uint UnitId { get; set; }
        public CombatMomentumStat StatType { get; set; } // 5u
        public uint StreakAmount { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(UnitId);
            writer.Write(StatType, 5u);
            writer.Write(StreakAmount);
        }
    }
}
