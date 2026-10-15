using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerMatchingMatchPvpStateUpdated)]
    public class ServerMatchingMatchPvpStateUpdated : StateInfo
    {
    }
}
