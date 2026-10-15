using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.Immune)]
public sealed partial class CombatLogImmune : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.Immune;

    public CombatLogCastData CastData { get; set; }

    public void Write(GamePacketWriter writer)
    {
        CastData.Write(writer);
    }
}
