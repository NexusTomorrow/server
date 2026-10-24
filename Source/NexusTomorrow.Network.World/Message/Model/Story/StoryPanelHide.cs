using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

/// <summary>
/// Hides the story panel; the client raises StoryPanelDialog_Hide.
/// </summary>
[WireMessage(GameMessageOpcode.Server0759, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class StoryPanelHide
{
}
