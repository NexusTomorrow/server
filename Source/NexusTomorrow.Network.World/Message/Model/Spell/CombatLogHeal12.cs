using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 12, which reports a heal.
/// </summary>
[WireStruct]
[UnionCase(12)]
public sealed partial class CombatLogHeal12 : CombatLogHeal
{
}
