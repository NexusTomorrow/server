using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

/// <summary>
/// Unlocks one pet flair; the client adds it to the unlocked flair of its type, shows a chat line and raises PetFlairUnlocked.
/// </summary>
[WireMessage(GameMessageOpcode.Server068D, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PetFlairUnlocked
{
    /// <summary>
    /// The flair, as an id the client looks up in its PetFlair table.
    /// </summary>
    [UInt(14)] public ushort PetFlairId { get; set; }
}
