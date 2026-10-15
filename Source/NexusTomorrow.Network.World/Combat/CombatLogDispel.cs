using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
public sealed partial class CombatLogDispel : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.Dispel;

    [UInt(1)] public bool BRemovesSingleInstance { get; set; }
    [UInt(32)] public uint InstancesRemoved { get; set; }
    [UInt(18)] public uint SpellRemovedId { get; set; } // 18u

    public void Write(GamePacketWriter writer)
    {
        writer.Write(BRemovesSingleInstance);
        writer.Write(InstancesRemoved);
        writer.Write(SpellRemovedId, 18u);
    }
}
