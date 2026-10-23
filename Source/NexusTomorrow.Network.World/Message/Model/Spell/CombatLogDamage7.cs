using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 7, which reports damage.
/// </summary>
[WireStruct]
[UnionCase(7)]
public sealed partial class CombatLogDamage7 : CombatLogDamage
{
}
