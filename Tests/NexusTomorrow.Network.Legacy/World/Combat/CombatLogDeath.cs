using NexusTomorrow.Game.Static.Combat;

namespace NexusTomorrow.Network.World.Combat;

public class CombatLogDeath : ICombatLog
{
    public CombatLogType Type => CombatLogType.Death;

    public uint UnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
    }
}
