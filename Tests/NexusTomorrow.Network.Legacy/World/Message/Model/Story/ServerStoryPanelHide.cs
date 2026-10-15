using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[Message(GameMessageOpcode.ServerStoryPanelHide)]
public class ServerStoryPanelHide : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // zero byte message
    }
}
