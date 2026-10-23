using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 35, which reports one amount.
/// </summary>
[WireStruct]
[UnionCase(35)]
public sealed partial class CombatLogAmount35 : CombatLogAmount
{
}
