using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
public sealed partial class CombatLogStealth : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.Stealth;

    [UInt(32)] public uint UnitId { get; set; }
    [UInt(1)] public bool BExiting { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(UnitId);
        writer.Write(BExiting);
    }
}
