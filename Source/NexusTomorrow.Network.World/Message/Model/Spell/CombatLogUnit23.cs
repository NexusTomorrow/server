using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 23, which names only a unit.
/// </summary>
[WireStruct]
[UnionCase(23)]
public sealed partial class CombatLogUnit23 : CombatLogUnit
{
}
