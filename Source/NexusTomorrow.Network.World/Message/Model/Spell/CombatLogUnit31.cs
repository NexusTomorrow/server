using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 31, which names only a unit.
/// </summary>
[WireStruct]
[UnionCase(31)]
public sealed partial class CombatLogUnit31 : CombatLogUnit
{
}
