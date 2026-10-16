using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Tells character select the login queue is done.
/// </summary>
/// <remarks>
/// Taken in any state; the client passes it to its interface.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0715, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class LoginQueueFinished
{
}
