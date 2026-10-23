using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

[WireStruct]
[UnionCase(4)]
public sealed partial class CombatLogDamage4 : CombatLogDamage
{
}
