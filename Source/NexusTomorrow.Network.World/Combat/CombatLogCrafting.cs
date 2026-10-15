using NexusTomorrow.Game.Static.Combat;

namespace NexusTomorrow.Network.World.Combat
{
    public class CombatLogCrafting : ICombatLog
    {
        public CombatLogType Type => CombatLogType.Crafting;

        public uint CasterUnitId { get; set; }
        public uint Item2Id { get; set; } // 18u

        public void Write(GamePacketWriter writer)
        {
            writer.Write(CasterUnitId);
            writer.Write(Item2Id, 18u);
        }
    }
}
