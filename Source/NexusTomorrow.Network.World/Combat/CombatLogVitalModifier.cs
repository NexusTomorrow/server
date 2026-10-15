using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Game.Static.Entity;

namespace NexusTomorrow.Network.World.Combat
{
    public class CombatLogVitalModifier : ICombatLog
    {
        public CombatLogType Type => CombatLogType.VitalModifier;

        public float Amount { get; set; }
        public Vital VitalModified { get; set; }
        public bool BShowCombatLog { get; set; }
        public CombatLogCastData CastData { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Amount);
            writer.Write(VitalModified, 5u);
            writer.Write(BShowCombatLog);
            CastData.Write(writer);
        }
    }
}
