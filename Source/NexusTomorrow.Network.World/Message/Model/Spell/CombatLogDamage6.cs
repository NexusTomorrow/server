using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 6, which reports damage.
/// </summary>
/// <remarks>
/// The client ignores <see cref="CombatLogDamage.EffectType"/>.
/// </remarks>
[WireStruct]
[UnionCase(6)]
public sealed partial class CombatLogDamage6 : CombatLogDamage
{
}
