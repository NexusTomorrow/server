using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Map;

/// <summary>
/// Clears the destination marker that <see cref="DestinationMarkerSet"/> sets.
/// </summary>
[WireMessage(GameMessageOpcode.Server07E4, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class DestinationMarkerCleared
{
}
