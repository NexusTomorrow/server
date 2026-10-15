using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Story.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[Message(GameMessageOpcode.ServerStoryTextUnitYell)]
[WireMessage(GameMessageOpcode.ServerStoryTextUnitYell, Direction.ServerToClient)]
public sealed partial class ServerStoryTextUnitYell : IWritable
{
    public StoryMessage StoryMessage { get; set; }
    [UInt(32)] public uint UnitId { get; set; }
    [UInt(18)] public uint Creature2Id { get; set; } // uses this if Unit has not been loaded to client yet
    [Float32] public float TextBubbleRange { get; set; }

    public void Write(GamePacketWriter writer)
    {
        StoryMessage.Write(writer);
        writer.Write(UnitId);
        writer.Write(Creature2Id, 18u);
        writer.Write(TextBubbleRange);
    }
}
