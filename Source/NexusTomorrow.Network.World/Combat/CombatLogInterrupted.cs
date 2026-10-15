using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Game.Static.Spell;

namespace NexusTomorrow.Network.World.Combat
{
    public class CombatLogInterrupted : ICombatLog
    {
        public CombatLogType Type => CombatLogType.Interrupted;

        public uint InterruptingSpellId { get; set; } // 18u
        public CastResult CastResult { get; set; } // 9u
        public CombatLogCastData CastData { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(InterruptingSpellId, 18u);
            writer.Write(CastResult, 9u);
            CastData.Write(writer);
        }
    }
}
