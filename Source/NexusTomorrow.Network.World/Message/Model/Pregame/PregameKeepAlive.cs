using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Keeps the connection alive on character and realm select.
/// </summary>
/// <remarks>
/// The client sends it about once a minute (a row of its game formula table, 60 seconds by default) while the player is using the
/// keyboard or mouse.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0241, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class PregameKeepAlive
{
}
