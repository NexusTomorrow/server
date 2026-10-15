using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.Modifying)]
public sealed partial class CombatLogModifying : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.Modifying;

    [UInt(32)] public uint CasterId { get; set; }
    [UInt(18)] public uint HostItem2Id { get; set; } // 18u
    [UInt(32)] public uint ItemRemovedItem2Id { get; set; }
    [UInt(32)] public uint ItemAddedItem2Id { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CasterId);
        writer.Write(HostItem2Id, 18u);
        writer.Write(ItemRemovedItem2Id);
        writer.Write(ItemAddedItem2Id);
    }
}
