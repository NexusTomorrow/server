using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 22, which names only a unit.
/// </summary>
[WireStruct]
[UnionCase(22)]
public sealed partial class CombatLogUnit22 : CombatLogUnit
{
}
