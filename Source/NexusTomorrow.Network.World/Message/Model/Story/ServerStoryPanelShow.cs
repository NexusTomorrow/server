using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Story.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[WireMessage(GameMessageOpcode.ServerStoryPanelShow, Direction.ServerToClient)]
public sealed partial class ServerStoryPanelShow
{
    public StoryMessage StoryMessage { get; set; }
}
