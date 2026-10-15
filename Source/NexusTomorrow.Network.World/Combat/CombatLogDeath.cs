using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
public sealed partial class CombatLogDeath : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.Death;

    [UInt(32)] public uint UnitId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
    }
}
