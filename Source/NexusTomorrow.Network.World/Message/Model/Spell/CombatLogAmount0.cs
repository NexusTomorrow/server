using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// A combat log entry of kind 0, which reports one amount.
/// </summary>
[WireStruct]
[UnionCase(0)]
public sealed partial class CombatLogAmount0 : CombatLogAmount
{
}
