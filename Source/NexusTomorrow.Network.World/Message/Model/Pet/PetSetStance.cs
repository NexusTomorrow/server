using NexusTomorrow.Game.Static.Pet;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

/// <summary>
/// Asks to set the stance of one of the player's pets, or of every pet the player has.
/// </summary>
[WireMessage(GameMessageOpcode.Client068E, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class PetSetStance
{
    /// <summary>
    /// The pet, by <see cref="SummonedPet.UnitId"/>, or 0 for every pet the player has.
    /// </summary>
    [UInt(32)] public uint UnitId { get; private set; }

    /// <summary>
    /// The stance asked for, as one stance of the set.
    /// </summary>
    [UInt(5)] public PetStances Stance { get; private set; }
}
