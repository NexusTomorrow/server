using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
public sealed partial class CombatLogLifesteal : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.Lifesteal;

    [UInt(32)] public uint UnitId { get; set; }
    [UInt(32)] public uint HealthStolen { get; set; }
    [UInt(32)] public uint Absorption { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(HealthStolen);
        writer.Write(Absorption);
    }
}
