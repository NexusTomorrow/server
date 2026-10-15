using NexusTomorrow.Game.Static.Spell;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class TargetInfo // same used for 0x0818
{
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(8)] public byte Ndx { get; set; }
    [UInt(8)] public byte TargetFlags { get; set; }
    [UInt(16)] public ushort InstanceCount { get; set; }
    [UInt(4)] public CombatResult CombatResult { get; set; }

    [Array(CountBits = 8)] public List<EffectInfo> EffectInfoData { get; set; } = [];

}
