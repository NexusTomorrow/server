using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

/// <summary>
/// Asks to summon one of the player's vanity pets. The client sends it only for a vanity pet whose ability the player holds.
/// </summary>
[WireMessage(GameMessageOpcode.Client082F, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class SummonVanityPet
{
    /// <summary>
    /// The vanity pet, as the Spell4Base id of its spell: the id the collectibles list gives the pet, which the client resolves to the
    /// spell's tiers.
    /// </summary>
    [UInt(18)] public uint Spell4BaseId { get; private set; }
}
