using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[Message(GameMessageOpcode.ServerStoryPanelHide)]
[WireMessage(GameMessageOpcode.ServerStoryPanelHide, Direction.ServerToClient)]
public sealed partial class ServerStoryPanelHide : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // zero byte message
    }
}
