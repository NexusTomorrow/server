using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
public sealed partial class CombatLogHealingAbsorption : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.HealingAbsorption;

    [UInt(32)] public uint Amount { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Amount);
    }
}
