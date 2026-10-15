using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerEntityStatUpdateInteger)]
    public class ServerEntityStatUpdateInteger : ServerEntityStatUpdateFloat
    {
    }
}
