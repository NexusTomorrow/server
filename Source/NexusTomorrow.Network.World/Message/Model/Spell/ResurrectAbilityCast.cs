using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Casts the player's class resurrection ability on the dead player targeted.
/// </summary>
/// <remarks>
/// The ability is the Class table's spell4IdResAbility for the player's class.
/// </remarks>
[WireMessage(GameMessageOpcode.Client00E4, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ResurrectAbilityCast
{
}
