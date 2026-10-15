using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Story.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

// If neither IsEmote or IsTextBubble is set, the text is made and NPC Say in the chat log with a chat bubble
// If either is set, no NPC say is created. Both IsEmote and IsTextBubble can be set at the same time.
[WireMessage(GameMessageOpcode.ServerStoryTextUnit, Direction.ServerToClient)]
public sealed partial class ServerStoryTextUnit
{
    public StoryMessage StoryMessage { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(1)] public bool IsEmote { get; set; }
    [UInt(1)] public bool IsTextBubble { get; set; }
    [Float32] public float TextBubbleRange { get; set; }
}
