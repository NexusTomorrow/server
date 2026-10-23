using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(17)]
public sealed partial class CombatLogDeflect : ICombatLogPayload
{
    [UInt(1)] public bool Unknown0 { get; set; }
    public CombatLogAction Unknown1 { get; set; } = new();
}
