using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.VitalModifier)]
public sealed partial class CombatLogVitalModifier : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.VitalModifier;

    [Float32] public float Amount { get; set; }
    [UInt(5)] public Vital VitalModified { get; set; }
    [UInt(1)] public bool BShowCombatLog { get; set; }
    public CombatLogCastData CastData { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Amount);
        writer.Write(VitalModified, 5u);
        writer.Write(BShowCombatLog);
        CastData.Write(writer);
    }
}
