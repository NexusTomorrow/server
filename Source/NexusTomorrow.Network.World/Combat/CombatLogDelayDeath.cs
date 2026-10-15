using NexusTomorrow.Game.Static.Combat;

namespace NexusTomorrow.Network.World.Combat
{
    public class CombatLogDelayDeath : ICombatLog
    {
        public CombatLogType Type => CombatLogType.DelayDeath;

        public CombatLogCastData CastData { get; set; }

        public void Write(GamePacketWriter writer)
        {
            CastData.Write(writer);
        }
    }
}
