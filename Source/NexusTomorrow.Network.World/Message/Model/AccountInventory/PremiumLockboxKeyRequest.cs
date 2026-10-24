using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.AccountInventory;

/// <summary>
/// Asks for the premium lockbox key.
/// </summary>
[WireMessage(GameMessageOpcode.Client079E, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class PremiumLockboxKeyRequest
{
}
