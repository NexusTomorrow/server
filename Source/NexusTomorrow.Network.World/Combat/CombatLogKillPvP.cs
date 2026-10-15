using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.KillPvP)]
public sealed partial class CombatLogKillPvP : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.KillPvP;

    public CombatLogCastData CastData { get; set; }

    public void Write(GamePacketWriter writer)
    {
        CastData.Write(writer);
    }
}
