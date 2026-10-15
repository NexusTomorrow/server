using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Story.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

// If neither IsEmote or IsTextBubble is set, the text is made and NPC Say in the chat log with a chat bubble
// If either is set, no NPC say is created. Both IsEmote and IsTextBubble can be set at the same time.
[Message(GameMessageOpcode.ServerStoryTextUnit)]
[WireMessage(GameMessageOpcode.ServerStoryTextUnit, Direction.ServerToClient)]
public sealed partial class ServerStoryTextUnit : IWritable
{
    public StoryMessage StoryMessage { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(1)] public bool IsEmote { get; set; }
    [UInt(1)] public bool IsTextBubble { get; set; }
    [Float32] public float TextBubbleRange { get; set; }

    public void Write(GamePacketWriter writer)
    {
        StoryMessage.Write(writer);
        writer.Write(UnitId);
        writer.Write(IsEmote);
        writer.Write(IsTextBubble);
        writer.Write(TextBubbleRange);
    }
}
