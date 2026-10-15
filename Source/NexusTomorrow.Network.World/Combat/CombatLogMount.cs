using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.Mount)]
public sealed partial class CombatLogMount : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.Mount;

    [UInt(1)] public bool BDismounted { get; set; }
    public CombatLogCastData CastData { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(BDismounted);
        CastData.Write(writer);
    }
}
