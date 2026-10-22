using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Guild;

[WireMessage(GameMessageOpcode.Server04CA, Direction.ServerToClient)]
public sealed partial class Server04CA : GuildOperationResult
{
}
