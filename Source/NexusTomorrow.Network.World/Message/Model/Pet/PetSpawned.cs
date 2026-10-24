using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

/// <summary>
/// Adds a pet the player has summoned; the client keeps it under its unit and raises PetSpawned with the unit.
/// </summary>
[WireMessage(GameMessageOpcode.Server0068, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class PetSpawned
{
    /// <summary>
    /// The pet.
    /// </summary>
    public SummonedPet Pet { get; set; } = new();
}
