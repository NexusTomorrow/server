using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

/// <summary>
/// Clears every pet flair the player has unlocked; the client raises PetFlairCleared.
/// </summary>
[WireMessage(GameMessageOpcode.Server023B, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PetFlairCleared
{
}
