using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Game.Static.Entity;

namespace NexusTomorrow.Network.World.Combat;

public class CombatLogDatacube : ICombatLog
{
    public CombatLogType Type => CombatLogType.Datacube;

    public uint UnitId { get; set; }
    public DatacubeType DatacubeType { get; set; } // 3u
    public bool HasPieces { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(DatacubeType, 3u);
        writer.Write(HasPieces);
    }
}
