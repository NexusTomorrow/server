using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(20)]
public sealed partial class CombatLogKillStreak : ICombatLogPayload
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(5)] public byte Unknown1 { get; set; }
    [UInt(32)] public uint Unknown2 { get; set; }
}
