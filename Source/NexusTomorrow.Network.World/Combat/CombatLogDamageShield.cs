using NexusTomorrow.Game.Static.Combat;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Combat;

[WireStruct]
public sealed partial class CombatLogDamageShield : CombatLogDamage
{
    [NotOnWire] public override CombatLogType Type => CombatLogType.DamageShields;
}
