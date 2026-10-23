using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(33)]
public sealed partial class CombatLogDatacube : ICombatLogPayload
{
    [UInt(32)] public uint Unknown0 { get; set; }
    [UInt(3)] public byte Unknown1 { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
}
