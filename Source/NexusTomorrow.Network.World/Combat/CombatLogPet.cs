using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.Pet)]
public sealed partial class CombatLogPet : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.Pet;

    [UInt(1)] public bool BDismissed { get; set; }
    [UInt(1)] public bool BKilled { get; set; }
    public CombatLogCastData CastData { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(BDismissed);
        writer.Write(BKilled);
        CastData.Write(writer);
    }
}
