using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 7, which reports damage.
/// </summary>
/// <remarks>
/// The client ignores <see cref="CombatLogDamage.GlanceAmount"/> and <see cref="CombatLogDamage.EffectType"/>; the glance amount the
/// add-ons see for this kind does not come from this message.
/// </remarks>
[WireStruct]
[UnionCase(7)]
public sealed partial class CombatLogDamage7 : CombatLogDamage
{
}
