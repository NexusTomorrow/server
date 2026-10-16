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
[Verified(Rung.Fields)]
public sealed partial class TransferDestinationsRequest
{
    /// <summary>
    /// True when the player asks from the interface's paid transfer, false from its free transfer.
    /// </summary>
    [UInt(1)] public bool Paid { get; private set; }
}
