using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireMessage(GameMessageOpcode.Server0501, Direction.ServerToClient)]
public sealed partial class Server0501 : Server0501Part0
{
}
