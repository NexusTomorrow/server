using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Asks the client to refresh what it shows of the player's path: it raises PlayerPathRefresh when the player exists.
/// </summary>
[WireMessage(GameMessageOpcode.Server06B4, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class PathRefresh
{
}
