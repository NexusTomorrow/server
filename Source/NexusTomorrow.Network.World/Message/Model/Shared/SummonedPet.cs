using NexusTomorrow.Game.Static.Pet;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// A pet the player has summoned, as <see cref="Pet.PetSpawned"/> adds one and <see cref="WorldEntry.PlayerState.Pets"/> lists them. The
/// client keeps each pet under its <see cref="UnitId"/>.
/// </summary>
[WireStruct]
public sealed partial class SummonedPet
{
    /// <summary>
    /// The pet's unit, by unit id. The client keeps the pet under it, and GameLib.GetPlayerPets returns the unit.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The spell that summoned the pet, as a Spell4 id. Where the client shows or casts a spell (its icon and action button, the ability
    /// book and collectibles lists, the cast), it finds the pet that spell summoned by this id.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint SummonSpell4Id { get; set; }

    [UInt(5)] public byte Unknown2 { get; set; }

    /// <summary>
    /// The pet's stance, as one stance of the set. Where more than one is set, the client takes the lowest.
    /// </summary>
    [VerifiedMember, UInt(5)] public PetStances Stance { get; set; }
}
