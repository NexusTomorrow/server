using NexusTomorrow.Game.Static.Combat;

namespace NexusTomorrow.Network.World.Combat
{
    public class CombatLogImmune : ICombatLog
    {
        public CombatLogType Type => CombatLogType.Immune;

        public CombatLogCastData CastData { get; set; }

        public void Write(GamePacketWriter writer)
        {
            CastData.Write(writer);
        }
    }
}
