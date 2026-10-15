using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

// Triggers VanityPetUnlocked ui event but there is no Carbine UI handler that uses this
// Triggers chatlog message "You added $1n to your Collectibles." with $1n filled in from the spell4Id
[WireMessage(GameMessageOpcode.ServerUnlockVanityPet, Direction.ServerToClient)]
public sealed partial class ServerUnlockVanityPet
{
    [UInt(18)] public uint Spell4Id { get; set; }
}

