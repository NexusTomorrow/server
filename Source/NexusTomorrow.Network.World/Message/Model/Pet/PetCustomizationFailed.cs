using NexusTomorrow.Game.Static.Pet;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

/// <summary>
/// Reports that a pet customization failed; the client raises PetCustomizationFailed with the result.
/// </summary>
[WireMessage(GameMessageOpcode.Server068B, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PetCustomizationFailed
{
    /// <summary>
    /// Why the customization failed.
    /// </summary>
    [VerifiedMember, UInt(4)] public PetCustomizeResult Result { get; set; }

    /// <summary>
    /// The pet type, raised as ePetType in the client's PetCustomizationFailed event.
    /// </summary>
    [VerifiedMember, UInt(2)] public byte PetType { get; set; }

    /// <summary>
    /// The pet, raised as idPet in the client's PetCustomizationFailed event.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint PetId { get; set; }

    /// <summary>
    /// The flair slot, raised as nFlairSlotIndex in the client's PetCustomizationFailed event.
    /// </summary>
    [VerifiedMember, UInt(16)] public ushort FlairSlotIndex { get; set; }

    /// <summary>
    /// The flair, as an id the client looks up in its PetFlair table.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort PetFlairId { get; set; }
}
