using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(21)]
public sealed partial class CombatLogKillPvp : ICombatLogPayload
{
    public CombatLogAction Unknown0 { get; set; } = new();
}
