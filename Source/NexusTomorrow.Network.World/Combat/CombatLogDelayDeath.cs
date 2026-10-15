using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
public sealed partial class CombatLogDelayDeath : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.DelayDeath;

    public CombatLogCastData CastData { get; set; }

    public void Write(GamePacketWriter writer)
    {
        CastData.Write(writer);
    }
}
