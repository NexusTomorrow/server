using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 4, which reports damage.
/// </summary>
[WireStruct]
[UnionCase(4)]
public sealed partial class CombatLogDamage4 : CombatLogDamage
{
}
