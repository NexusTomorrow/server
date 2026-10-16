using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Asks for the realms a character could transfer to.
/// </summary>
/// <remarks>
/// The client sends it only while the account's subscription has not expired (0x033D); the answer is
/// <see cref="TransferDestinationList"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client03EE, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class TransferDestinationsRequest
{
    /// <summary>
    /// The client gives this value no meaning. It sends whatever its interface passes.
    /// </summary>
    [UInt(1)] public bool Unknown0 { get; private set; }
}
