using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

/// <summary>
/// Asks to put a flair in one of a pet's slots, or to clear the slot. The client sends it only once its own check that the flair may go in
/// the slot passes.
/// </summary>
[WireMessage(GameMessageOpcode.Client07ED, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class PetCustomize
{
    /// <summary>
    /// The kind of pet, by <see cref="PetCustomization.PetType"/>.
    /// </summary>
    [UInt(2)] public PetType PetType { get; private set; }

    /// <summary>
    /// The customized object, by <see cref="PetCustomization.ObjectId"/>.
    /// </summary>
    [UInt(32)] public uint ObjectId { get; private set; }

    /// <summary>
    /// The slot, 0 to 3, an index into <see cref="PetCustomization.PetFlairIds"/>.
    /// </summary>
    [UInt(16)] public ushort FlairSlotIndex { get; private set; }

    /// <summary>
    /// The flair to put in the slot, by PetFlair id, or 0 to clear the slot.
    /// </summary>
    [UInt(14)] public ushort PetFlairId { get; private set; }
}
