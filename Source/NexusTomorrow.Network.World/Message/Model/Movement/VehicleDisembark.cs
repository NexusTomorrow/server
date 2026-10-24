using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Movement;

/// <summary>
/// Asks the server to take the player out of their vehicle. GameLib.Disembark sends it.
/// </summary>
/// <remarks>
/// The client sends it only while the player is in a vehicle the client lets them leave, and at most once every 3 seconds.
/// </remarks>
[WireMessage(GameMessageOpcode.Client01AF, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class VehicleDisembark
{
}
