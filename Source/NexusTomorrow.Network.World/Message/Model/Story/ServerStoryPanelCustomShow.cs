using NexusTomorrow.Game.Static.Story;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Story.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

[Message(GameMessageOpcode.ServerStoryPanelCustomShow)]
[WireMessage(GameMessageOpcode.ServerStoryPanelCustomShow, Direction.ServerToClient)]
public sealed partial class ServerStoryPanelCustomShow : IWritable
{
    public StoryMessage StoryMessage { get; set; }
    [UInt(32)] public uint SoundContextEventId { get; set; }
    [UInt(32)] public StoryPanelType StoryPanelType { get; set; }
    [UInt(32)] public uint DurationMS { get; set; }
    [UInt(32)] public StoryPanelStyle StoryPanelStyle { get; set; }

    public void Write(GamePacketWriter writer)
    {
        StoryMessage.Write(writer);
        writer.Write(SoundContextEventId);
        writer.Write(StoryPanelType, 32u);
        writer.Write(DurationMS);
        writer.Write(StoryPanelStyle, 32u);
    }
}
