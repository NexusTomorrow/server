using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

/// <summary>
/// Tells the client the player unlocked a vanity pet; it shows a chat line naming the pet's spell and raises VanityPetUnlocked.
/// </summary>
[WireMessage(GameMessageOpcode.Server01AE, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class VanityPetLearned
{
    /// <summary>
    /// The vanity pet's spell, as a Spell4 id the client looks up for the chat line's name.
    /// </summary>
    [UInt(18)] public uint Spell4Id { get; set; }
}
