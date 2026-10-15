using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingMatchPvpStateUpdated)]
[WireMessage(GameMessageOpcode.ServerMatchingMatchPvpStateUpdated, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchPvpStateUpdated : StateInfo
{
}
