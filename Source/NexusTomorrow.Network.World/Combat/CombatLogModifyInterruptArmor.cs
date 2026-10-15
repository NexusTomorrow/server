using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
public sealed partial class CombatLogModifyInterruptArmor : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.ModifyInterruptArmor;

    [UInt(32)] public uint Amount { get; set; }
    public CombatLogCastData CastData { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Amount);
        CastData.Write(writer);
    }
}
