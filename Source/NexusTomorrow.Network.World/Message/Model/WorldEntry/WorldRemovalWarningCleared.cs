using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.WorldEntry;

[WireMessage(GameMessageOpcode.Server0687, Direction.ServerToClient)]
public sealed partial class WorldRemovalWarningCleared
{
}
