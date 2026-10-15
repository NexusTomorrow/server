using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Game.Static.Combat.CrowdControl;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
public sealed partial class CombatLogCCState : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.CCState;

    [UInt(5)] public CCState State { get; set; } // 5u
    [UInt(1)] public bool BRemoved { get; set; }
    [UInt(32)] public uint InterruptArmorTaken { get; set; }
    [UInt(4)] public CCStateApplyRulesResult Result { get; set; } // 4u
    [UInt(14)] public ushort CcStateDiminishingReturnsId { get; set; } // 14u
    public CombatLogCastData CastData { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(State, 5u);
        writer.Write(BRemoved);
        writer.Write(InterruptArmorTaken);
        writer.Write(Result, 4u);
        writer.Write(CcStateDiminishingReturnsId, 14u);
        CastData.Write(writer);
    }
}
