using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(10)]
public sealed partial class CombatLogDelayDeath : ICombatLogPayload
{
    public CombatLogAction Unknown0 { get; set; } = new();
}
