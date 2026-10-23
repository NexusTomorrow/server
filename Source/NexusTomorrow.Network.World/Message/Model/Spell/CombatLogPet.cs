using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(26)]
public sealed partial class CombatLogPet : ICombatLogPayload
{
    [UInt(1)] public bool Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
    public CombatLogAction Unknown2 { get; set; } = new();
}
