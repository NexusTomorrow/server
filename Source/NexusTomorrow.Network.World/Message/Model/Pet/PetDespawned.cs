using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

/// <summary>
/// Removes a pet the player has summoned; the client raises PetDespawned with the pet's unit when it held the pet.
/// </summary>
[WireMessage(GameMessageOpcode.Server077F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PetDespawned
{
    /// <summary>
    /// The pet's unit, by <see cref="SummonedPet.UnitId"/>.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }
}
