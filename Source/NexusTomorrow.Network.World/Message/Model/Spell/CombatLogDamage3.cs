using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 3, which reports damage.
/// </summary>
[WireStruct]
[UnionCase(3)]
public sealed partial class CombatLogDamage3 : CombatLogDamage
{
}
