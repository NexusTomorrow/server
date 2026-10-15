using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.FallingDamage)]
public sealed partial class CombatLogFallingDamage : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.FallingDamage;

    [UInt(32)] public uint CasterId { get; set; }
    [UInt(32)] public uint Damage { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CasterId);
        writer.Write(Damage);
    }
}
