using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.Deflect)]
public sealed partial class CombatLogDeflect : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.Deflect;

    [UInt(1)] public bool BMultiHit { get; set; }
    public CombatLogCastData CastData { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(BMultiHit);
        CastData.Write(writer);
    }
}
