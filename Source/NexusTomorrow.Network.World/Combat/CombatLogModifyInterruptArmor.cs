using NexusTomorrow.Game.Static.Combat;

namespace NexusTomorrow.Network.World.Combat
{
    public class CombatLogModifyInterruptArmor : ICombatLog
    {
        public CombatLogType Type => CombatLogType.ModifyInterruptArmor;

        public uint Amount { get; set; }
        public CombatLogCastData CastData { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Amount);
            CastData.Write(writer);
        }
    }
}
