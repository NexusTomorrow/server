using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(18)]
public sealed partial class CombatLogImmunity : ICombatLogPayload
{
    public CombatLogAction Unknown0 { get; set; } = new();
}
