using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Asks for the realms a character could transfer to.
/// </summary>
/// <remarks>
/// The client sends it only while the account's subscription has not expired (<see cref="SubscriptionState"/>); the answer is
/// <see cref="TransferDestinationList"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client03EE, Direction.ClientToServer)]
[Verified(Rung.Purpose)]
public sealed partial class TransferDestinationsRequest
{
    [UInt(1)] public bool Unknown0 { get; private set; }
}
