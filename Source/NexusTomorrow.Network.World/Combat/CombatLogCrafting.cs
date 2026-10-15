using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
[UnionCase(CombatLogType.Crafting)]
public sealed partial class CombatLogCrafting : ICombatLog
{
    [NotOnWire] public CombatLogType Type => CombatLogType.Crafting;

    [UInt(32)] public uint CasterUnitId { get; set; }
    [UInt(18)] public uint Item2Id { get; set; } // 18u

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CasterUnitId);
        writer.Write(Item2Id, 18u);
    }
}
