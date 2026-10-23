using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 6, which reports damage.
/// </summary>
[WireStruct]
[UnionCase(6)]
public sealed partial class CombatLogDamage6 : CombatLogDamage
{
}
