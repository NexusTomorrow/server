using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

// Sets the spell's activated state.  Spells are considered activated when they are able to be added to the player's limited action set.
[WireMessage(GameMessageOpcode.ClientAbilityBookActivateSpell, Direction.ClientToServer)]
public sealed partial class ClientAbilityBookActivateSpell
{
    [UInt(18)] public uint Spell4Id { get; private set; }
    [UInt(1)] public bool Active { get; private set; }
}
