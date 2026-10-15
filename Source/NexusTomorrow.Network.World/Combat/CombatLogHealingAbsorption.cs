using NexusTomorrow.Game.Static.Combat;

namespace NexusTomorrow.Network.World.Combat;

public class CombatLogHealingAbsorption : ICombatLog
{
    public CombatLogType Type => CombatLogType.HealingAbsorption;

    public uint Amount { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Amount);
    }
}
