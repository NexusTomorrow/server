using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 13, which reports a heal.
/// </summary>
[WireStruct]
[UnionCase(13)]
public sealed partial class CombatLogHeal13 : CombatLogHeal
{
}
