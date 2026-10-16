using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Asks for the character list.
/// </summary>
/// <remarks>
/// The client sends it each time it enters character select: after a world connection's greeting, on choosing the current realm or
/// backing out of realm select, and after leaving the world.
/// </remarks>
[WireMessage(GameMessageOpcode.Client07E0, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CharacterListRequest
{
}
