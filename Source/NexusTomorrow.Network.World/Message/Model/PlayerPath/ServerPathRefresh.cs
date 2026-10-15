using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;


[WireMessage(GameMessageOpcode.ServerPathRefresh, Direction.ServerToClient)]
public sealed partial class ServerPathRefresh
{
}
