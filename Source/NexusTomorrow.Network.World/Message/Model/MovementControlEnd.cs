using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// Takes the movement of the unit the client moves back from the client.
/// </summary>
/// <remarks>
/// The client stops moving the unit, drops the movement it has gathered but not yet sent, and sends no more <see cref="ClientMovement"/>
/// until a <see cref="MovementControl"/> gives it a unit again. It then applies <see cref="UnitMovement"/> to the unit.
/// </remarks>
[WireMessage(GameMessageOpcode.Message0639, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MovementControlEnd
{
}
