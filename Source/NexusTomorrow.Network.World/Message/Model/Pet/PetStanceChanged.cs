using NexusTomorrow.Game.Static.Pet;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

/// <summary>
/// Sets the stance of a pet the player has summoned; the client raises PetStanceChanged with the pet's unit.
/// </summary>
[WireMessage(GameMessageOpcode.Server068F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PetStanceChanged
{
    /// <summary>
    /// The pet's unit, by <see cref="SummonedPet.UnitId"/>.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The pet's new stance, as <see cref="SummonedPet.Stance"/> holds it.
    /// </summary>
    [UInt(5)] public PetStances Stance { get; set; }
}
