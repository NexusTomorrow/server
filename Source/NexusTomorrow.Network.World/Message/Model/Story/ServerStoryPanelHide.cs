using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[WireMessage(GameMessageOpcode.ServerStoryPanelHide, Direction.ServerToClient)]
public sealed partial class ServerStoryPanelHide
{
}
