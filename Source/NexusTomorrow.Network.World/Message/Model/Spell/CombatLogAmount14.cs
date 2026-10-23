using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 14, which reports one amount.
/// </summary>
[WireStruct]
[UnionCase(14)]
public sealed partial class CombatLogAmount14 : CombatLogAmount
{
}
