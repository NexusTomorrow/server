using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
public sealed partial class CombatLogLAS : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.LAS;

    [UInt(32)] public uint UnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
    }
}
