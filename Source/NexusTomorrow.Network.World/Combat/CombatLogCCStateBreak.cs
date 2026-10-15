using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Game.Static.Combat.CrowdControl;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.CCStateBreak)]
public sealed partial class CombatLogCCStateBreak : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.CCStateBreak;

    [UInt(32)] public uint CasterId { get; set; }
    [UInt(5)] public CCState State { get; set; } // 5u

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CasterId);
        writer.Write(State, 5u);
    }
}
