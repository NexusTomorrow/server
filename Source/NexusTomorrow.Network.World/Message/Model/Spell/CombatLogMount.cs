using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(25)]
public sealed partial class CombatLogMount : ICombatLogPayload
{
    [UInt(1)] public bool Unknown0 { get; set; }
    public CombatLogAction Unknown1 { get; set; } = new();
}
