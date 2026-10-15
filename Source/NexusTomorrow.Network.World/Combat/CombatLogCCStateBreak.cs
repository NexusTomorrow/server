using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Game.Static.Combat.CrowdControl;

namespace NexusTomorrow.Network.World.Combat;

public class CombatLogCCStateBreak : ICombatLog
{
    public CombatLogType Type => CombatLogType.CCStateBreak;

    public uint CasterId { get; set; }
    public CCState State { get; set; } // 5u

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CasterId);
        writer.Write(State, 5u);
    }
}
