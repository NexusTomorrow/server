using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

// Seems be to used after a cinematic
[WireMessage(GameMessageOpcode.ServerLoadingScreen, Direction.ServerToClient)]
public sealed partial class ServerLoadingScreen
{
}
