using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
public sealed partial class CombatLogAbsorption : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.Absorption;

    [UInt(32)] public uint AbsorptionAmount { get; set; }
    public CombatLogCastData CastData { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(AbsorptionAmount);
        CastData.Write(writer);
    }
}
