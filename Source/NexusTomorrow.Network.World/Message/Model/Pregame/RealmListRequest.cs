using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Asks for the realm list when the player opens realm select from character select.
/// </summary>
/// <remarks>
/// Sent on the character-select connection, which realm select takes over.
/// </remarks>
[WireMessage(GameMessageOpcode.Client07A4, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class RealmListRequest
{
}
