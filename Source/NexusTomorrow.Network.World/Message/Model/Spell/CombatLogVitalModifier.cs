using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(16)]
public sealed partial class CombatLogVitalModifier : ICombatLogPayload
{
    [Float32] public float Unknown0 { get; set; }
    [UInt(5)] public byte Unknown1 { get; set; }
    [UInt(1)] public bool Unknown2 { get; set; }
    public CombatLogAction Unknown3 { get; set; } = new();
}
